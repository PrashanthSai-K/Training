create table rental_tax_log (
    rental_id int,
    customer_name text,
    rental_date timestamp,
    amount numeric,
    tax numeric
);

select * from rental_tax_log

do $$
declare
    rec record;
    cur cursor for
        select r.rental_id, 
               c.first_name || ' ' || c.last_name as customer_name,
               r.rental_date,
               p.amount 
        from rental r
        join payment p on r.rental_id = p.rental_id
        join customer c on r.customer_id = c.customer_id;
begin
    open cur;

    loop
        fetch cur into rec;
        exit when not found;

        insert into rental_tax_log (rental_id, customer_name, rental_date, amount, tax)
        values (
            rec.rental_id,
            rec.customer_name,
            rec.rental_date,
            rec.amount,
            rec.amount * 0.10
        );
    end loop;

    close cur;
end;
$$;



----------------------------CURSORS--------------------------------

-- 1. Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.

create table customer_rental_count(
id serial primary key,
customer_id int,
customer_name text,
rental_count int
);

select * from customer_rental_count;

do
$$
	declare 
		cus_record record;
		customer_cursor cursor for
		select * from customer;
		rent_count int;
		
	begin
		open customer_cursor;

		loop
			fetch next from customer_cursor into cus_record;
			exit when not found;
			
			select count(*) into rent_count from rental where customer_id = cus_record.customer_id;
	
			insert into customer_rental_count (customer_id, customer_name, rental_count) 
			values (cus_record.customer_id, concat(cus_record.first_name, ' ', cus_record.last_name), rent_count);
		end loop;

		close customer_cursor;
	end;
$$

select * from customer_rental_count;


-- 2. Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.


do
$$ 
	declare
		rent_count int;
		film_record record;
		film_cursor cursor for
		select f.film_id, f.title, c.name as category from film_category fc
		join film f on f.film_id = fc.film_id
		join category c on c.category_id = fc.category_id
		where c.name = 'Comedy';

	begin
		open film_cursor;

		loop
			fetch next from film_cursor into film_record;
			exit when not found;

				select count(*) into rent_count from rental r
				join inventory i on i.inventory_id = r.inventory_id
				where i.film_id =  film_record.film_id;


			if rent_count > 10 then
				raise notice 'Film Id : %, Film Title : %, Count : %',
				film_record.film_id, film_record.title, rent_count;				
			end if;
			
		end loop;
		close film_cursor;
	end;
$$ language plpgsql

-- 3. Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.

create table film_by_store(
id serial primary key,
store_id int,
film_id int
)

select * from film_by_store;

do
$$
	declare
		store_record record;
		inv_record record;
		film_count int;
		
		store_cursor cursor for
		select * from store;

		inv_cursor cursor(p_store_id int) for
		select distinct film_id from inventory where store_id = p_store_id;
		
	begin
		open store_cursor;
		loop
			fetch next from store_cursor into store_record;
			exit when not found;

			film_count := 0;
			
			open inv_cursor(store_record.store_id);
			loop
				fetch next from inv_cursor into inv_record;
				exit when not found;
				
				insert into film_by_store (store_id, film_id) values (store_record.store_id, inv_record.film_id);
				
				film_count := film_count+1;
			end loop;
			close inv_cursor;
			raise notice 'Store Id : %, Film Count : %', store_record.store_id, film_count;
			raise notice 'All distinct films for store % has been inserted successfully', store_record.store_id;
		end loop;
		close store_cursor;
	end;
$$ language plpgsql;

select * from film_by_store;

/*
OUTPUT : 

NOTICE:  Store Id : 1, Film Count : 759
NOTICE:  All distinct films for store 1 has been inserted successfully
NOTICE:  Store Id : 2, Film Count : 762
NOTICE:  All distinct films for store 2 has been inserted successfully
*/


-- 4. Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive_customers table.

create table inactive_customers(
id serial primary key,
customer_id int
);

select * from inactive_customers;

do
$$
	declare
		cust_rec record;
		cust_cursor cursor for
		select * from rental;

	begin
	open cust_cursor;
		loop
			fetch next from cust_cursor into cust_rec;
			exit when not found;
			
			if now() - cust_rec.rental_date >  interval '6 months' then
				insert into inactive_customers (customer_id) values (cust_rec.customer_id);
			end if;
			
		end loop;
	close cust_cursor;
	end;
$$ language plpgsql

select * from inactive_customers;


---------------------------------TRANSACTION----------------------------------------------

-- 1. Write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.

begin;

with new_customer as 
(
	insert into customer (store_id, first_name, last_name, email, address_id, active) values (1, 'sai p', 'rashanth k', 'prs@gmail.com', 1, 1)
	returning customer_id
),
new_rental as 
(
	insert into rental (rental_date, inventory_id, customer_id, return_date, staff_id) 
	values (now(), 1, (select customer_id from new_customer), now() + interval '2 days', 1)
)
select * from payment where customer_id = (select customer_id from new_customer);

commit;


-- 2. Simulate a transaction where one update fails (e.g., invalid rental ID), and ensure the entire transaction rolls back.


begin;

select * from payment where payment_id = 17504;

update payment set amount = amount * 2 where payment_id = 17504;

update payment set amount = amount - 1 where pament_id = 1; -- column name error so rollsback

rollback;


-- 3. Use SAVEPOINT to update multiple payment amounts. Roll back only one payment update using ROLLBACK TO SAVEPOINT.

select * from payment;

begin;

update payment set amount = amount * 2 where payment_id = 17504;

savepoint s1;

update payment set amount = amount * 2 where payment_id = 17505;

savepoint s2;

update payment set amount = amount * 2 where payment_id = 17506;

rollback to s1;

commit;

-- 4. Perform a transaction that transfers inventory from one store to another (delete + insert) safely.


select * from inventory where inventory_id = 2 and store_id = 1;

begin;

select * from inventory order by inventory_id desc

with transfer as
(
	select film_id from inventory where inventory_id = 2 and store_id = 1
),
inserted as
(
	insert into inventory (film_id, store_id) values ((select film_id from transfer), 1)
	returning inventory_id
)
update rental set inventory_id = (select inventory_id from inserted) where inventory_id = 2;

delete from inventory where inventory_id = 2 and store_id = 1;

commit;


-- 5. Create a transaction that deletes a customer and all associated records (rental, payment), ensuring referential integrity.


begin;

delete from payment where customer_id = 333;

delete from rental where customer_id = 333;

delete from customer where customer_id = 333;

commit;

------------------------------TRIGGERS---------------------------------

-- 1. Create a trigger to prevent inserting payments of zero or negative amount.

select * from payment order by payment_id desc

create or replace function prevent_insert_func()
returns trigger as
$$
	begin
		if(new.amount <= 0) then
			raise exception 'Payment amount cannot be 0';
			return null;
		end if;
		raise notice 'Inserted Successfully';
		return new;
	end
$$language plpgsql;

create trigger payment_insert_trigger
before insert on payment
for each row
execute function prevent_insert_func();

insert into payment (customer_id, staff_id, rental_id, amount, payment_date) values (1, 1, 1, 0, now())

-- 2. Set up a trigger that automatically updates last_update on the film table when the title or rental rate is changed.

select * from film where film_id = 1;

create or replace function update_film_func()
returns trigger as
$$
	begin
		if old.title is distinct from new.title or old.rental_rate is distinct from new.rental_rate then
			new.last_update = now();
		end if;
		return new;
	end;
$$language plpgsql;

create trigger update_film_trigger
after update on film
for each row
execute function update_film_func();

update film set rental_rate = 1.01 where film_id = 1;

-- 3. Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week.

create table rental_log(
id serial primary key,
film_id int, 
rental_id int 
)

select * from rental_log;

create or replace function insert_rental_log()
returns trigger
as
$$
	declare
		film_id_var int;
		film_count_var int;
	begin
		select i.film_id into film_id_var
		from inventory i
		where i.inventory_id = new.inventory_id;

		select count(*) into film_count_var from rental r 
		join inventory i on r.inventory_id = i.inventory_id
		where i.film_id = film_id_var and now() - rental_date < interval '1 week';

		if film_count_var > 3 then
			insert into rental_log (film_id, rental_id) values (film_id_var, new.rental_id);
		end if;
		return new;
	end;
$$ language plpgsql;

create trigger trg_insert_rental_log
after insert
on rental
for each row
execute function insert_rental_log();

insert into rental (rental_date, inventory_id, customer_id, return_date, staff_id) values (now(), 1711, 459, now() + interval '3 days', 1)

select * from rental order by rental_date desc;
select * from rental_log

