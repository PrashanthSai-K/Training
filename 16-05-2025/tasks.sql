-- You are tasked with building a PostgreSQL-backed database for an EdTech company that manages online training and certification programs for individuals across various technologies.


/*
Phase 1 :  Normalized Table Design :

Solution :

status:
id, name

states:
id, state

districts:
id, district, state_id

areas:
pincode, area, district_id

address:
id, line1, line2, area_id

students:
stu_id, name, email(unique), phone, address_id, active

trainer:
trainer_id, name, designation, email(unique), phone, address_id, active

domains: (like cloud, web app, mobile app, etc..)
domain_id, domain_name

sub_domains: (like aws, gcp, azure, csharp, etc..)
sub_domain_id, sub_domain_name, domain_id

courses:
course_id, name, description, sub_domain_id, course_length, fees, active

course_trainers:
course_trainer_id, course_id, trainer_id

course_subscriptions:
subscription_id, student_id, course_id, subscribe_duration, subscribed_on, status_id

certificates:
certificate_id, course_id, student_id, certified_on, valid_till

payments:
payment_id, subscription_id, payment_mode, amount, status_id, payment_on

*/


/*

Phase 2: DDL & DML

* Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)
* Insert sample data using `INSERT` statements
* Create indexes on `student_id`, `email`, and `course_id`

*/

/*
-- Creating tables

1. **students**

   * `student_id (PK)`, `name`, `email`, `phone`

*/

create table students (
	id serial primary key,
	name text not null,
	email text unique not null,
	phone text not null
);

/*

2. **courses**

   * `course_id (PK)`, `course_name`, `category`, `duration_days`

*/

create table courses  (
	course_id serial primary key,
	course_name text not null,
	category text not null,
	duration_days text not null
);

/*

3. **trainers**

   * `trainer_id (PK)`, `trainer_name`, `expertise`

*/

create table trainers (
	trainer_id serial primary key,
	trainer_name text not null,
	expertise text not null
);

/*

4. **enrollments**

   * `enrollment_id (PK)`, `student_id (FK)`, `course_id (FK)`, `enroll_date`

*/

create table enrollments (
	enrollment_id serial primary key,
	student_id int not null,
	course_id int not null,
	enroll_date timestamp default current_timestamp,
	constraint fk_student foreign key (student_id) references students(id),
	constraint fk_course foreign key (course_id) references courses(course_id)
);

/*

5. **certificates**

   * `certificate_id (PK)`, `enrollment_id (FK)`, `issue_date`, `serial_no`

*/

create table certificates (
	certificate_id serial primary key,
	enrollment_id int not null,
	issue_date timestamp default current_timestamp,
	serial_no text unique not null,
	constraint fk_enrollment foreign key (enrollment_id) references enrollments(enrollment_id)
);

/*

6. **course\_trainers** (Many-to-Many if needed)

   * `course_id`, `trainer_id`

*/

create table course_trainers (
	course_id  int not null,
	trainer_id int not null,
	constraint fk_course foreign key (course_id) references courses(course_id),
	constraint fk_trainer foreign key (trainer_id) references trainers(trainer_id)
);

-- Inserting sample datas

insert into students (name, email, phone) 
values 
	('sai prashanth k', 'sai@gmail.com', '1234567890'),
	('kavinraj k', 'kavin@gmail.com', '0987654321'),
	('hariharan p', 'hari@gmail.com', '6789054321');
	
insert into courses (course_name, category, duration_days)
values 
	('Learn fundamentals of Git', 'DevOps', '3 days'),
	('Learn to program with MYSQL', 'SQL', '7 days'),
	('Advanced programming with MYSQl', 'SQL', '7 days'),
	('Learn to program with React.js', 'Web App', '6 days'),
	('Build medium level application with React.js', 'Web App', '3 days');

insert into trainers (trainer_name, expertise)
values
	('keerthivasan', 'Java'),
	('varun', 'SQL'),
	('roganth', 'Javascript');

insert into enrollments (student_id, course_id)
values (1, 2), (2, 4), (3, 5);

insert into certificates (enrollment_id, serial_no)
values (1, gen_random_uuid()::text), (2, gen_random_uuid()::text), (3, gen_random_uuid()::text);

insert into course_trainers (course_id, trainer_id)
values (1, 1), (2, 2), (3, 2), (4, 3), (5, 3);

select * from students;
select * from courses;
select * from trainers;
select * from enrollments;
select * from certificates;

-- creating indexes

create index idx_student_id on enrollments(student_id);

create unique index idx_email on students(email);

create index idx_course_id on enrollments(course_id);

/*
Phase 3: SQL Joins Practice

Write queries to:

*/
-- 1. List students and the courses they enrolled in

select 
	s.name, 
	s.email, 
	c.course_name, 
	c.category, 
	c.duration_days from students s
left join enrollments e on e.student_id = s.id
left join courses c on c.course_id = e.course_id;

-- 2. Show students who received certificates with trainer names

select 
	s.name as student_name,
	cr.course_name,
	c.serial_no as certificate_no,
	c.issue_date as certified_date,
	t.trainer_name
from students s
left join enrollments e 
	on e.student_id = s.id
left join certificates c 
	on c.enrollment_id = e.enrollment_id
left join course_trainers ct
	on ct.course_id = e.course_id
left join courses cr 
	on cr.course_id = e.course_id
left join trainers t 
	on t.trainer_id = ct.trainer_id


-- 3. Count number of students per course

select 
	er.course_id,
	c.course_name,
	count(*) as student_count
from enrollments er
join courses c
	on c.course_id = er.course_id
join students s
	on s.id = er.student_id
group by er.course_id, c.course_name


/*
Phase 4: Functions & Stored Procedures

Function :

Create `get_certified_students(course_id INT)`
→ Returns a list of students who completed the given course and received certificates.

*/

create or replace function get_certified_students(p_course_id int)
returns table (
	name text
)
as
$$
	begin
		return query
		select s.name
		from students s
		join enrollments er
			on er.student_id = s.id
		join certificates c
			on c.enrollment_id = er.enrollment_id
		where er.course_id = p_course_id;
	end;
$$ language plpgsql;

select get_certified_students(5);

/*

Stored Procedure:

Create `sp_enroll_student(p_student_id, p_course_id)`
→ Inserts into `enrollments` and conditionally adds a certificate if completed (simulate with status flag).

*/

create or replace procedure sp_enroll_student(p_student_id int, p_course_id int, p_completed boolean)
as
$$
	declare
		v_enrollment_id int;
	begin
		if p_completed then
			insert into enrollments (student_id, course_id)
			values (p_student_id, p_course_id)
			returning enrollment_id into v_enrollment_id;

			insert into certificates (enrollment_id, serial_no)
			values (v_enrollment_id, gen_random_uuid()::text);
		else
			insert into enrollments (student_id, course_id)
			values (p_student_id, p_course_id);
		end if;
	end;
$$ language plpgsql;

call sp_enroll_student(2, 3, false);

select * from enrollments;
select * from certificates;

/*

Phase 5: Cursor

Use a cursor to:

* Loop through all students in a course
* Print name and email of those who do not yet have certificates

*/

create or replace procedure sp_uncertified_students()
as
$$
	declare
		stu_rec record;
		stu_cur cursor for
		select 
			s.name, 
			s.email, 
			cr.course_name,
			c.certificate_id
		from enrollments er
		left join students s 
			on s.id = er.student_id
		left join certificates c 
			on c.enrollment_id = er.enrollment_id
		left join courses cr 
			on cr.course_id = er.course_id;
		
	begin
		open stu_cur;

		loop
			fetch next from stu_cur into stu_rec;
			exit when not found;

			if stu_rec.certificate_id is null then
				raise notice 'Student Name: %, Email: %, Course Name: %', stu_rec.name, stu_rec.email, stu_rec.course_name;
			end if;
		end loop;
		close stu_cur;
	end;
$$ language plpgsql;

call sp_uncertified_students();


/*

Phase 6: Security & Roles

1. Create a `readonly_user` role:

   * Can run `SELECT` on `students`, `courses`, and `certificates`
   * Cannot `INSERT`, `UPDATE`, or `DELETE`

*/

create role readonly_user with login password 'password';

grant connect on database edtech to readonly_user;

grant select on students, courses, certificates to readonly_user;

/*

2. Create a `data_entry_user` role:

   * Can `INSERT` into `students`, `enrollments`
   * Cannot modify certificates directly
*/

create role data_entry_user with login password 'password';

grant connect on database edtech to data_entry_user;

grant insert on students, enrollments to data_entry_user;
grant update on students_id_seq, enrollments_enrollment_id_seq  to data_entry_user;


/*

Phase 7: Transactions & Atomicity

Write a transaction block that:

* Enrolls a student
* Issues a certificate
* Fails if certificate generation fails (rollback)

*/

-- Written a transaction block using stored procedure as it is a best practice for reusability

create or replace procedure sp_create_student(p_student_id int, p_course_id int)
as
$$
	declare
		v_enrollment_id int;
	begin
		insert into enrollments (student_id, course_id) 
		values (p_student_id, p_course_id)
		returning enrollment_id into v_enrollment_id;

		insert into certificates (enrollment_id, serial_no)
		values (v_enrollment_id, gen_random_uuid()::text);

		exception
			when others then
				raise notice 'Error occurred : %', sqlerrm;
	end;
$$ language plpgsql;


call sp_create_student(4, 3);

select * from certificates;
select * from enrollments;


