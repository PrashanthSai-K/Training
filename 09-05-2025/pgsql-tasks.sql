-- Write a cursor that loops through all films and prints titles longer than 120 minutes.

do
$$
	declare 
		film_record record;
		film_cursor cursor for
		select * from film;
	begin
		open film_cursor;
		
		loop
			fetch next from film_cursor into film_record;
			exit when not found;
		
			if film_record.length > 120 then
				raise notice 'Film Title : %, Length: %', film_record.title, film_record.length;
			end if;
		end loop;
		
		close film_cursor;
	end
$$

-- Create a cursor that iterates through all customers and counts how many rentals each made.

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
	
			raise notice 'Customer name : %, Rental Count : %', 
			concat(cus_record.first_name, ' ', cus_record.last_name), rent_count;

		end loop;

		close customer_cursor;
	end;

$$

-- Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.

select * from film where film_id = 904;

select i.film_id, count(i.film_id)as count from rental r
join inventory i on i.inventory_id = r.inventory_id
group by i.film_id
order by count


do
$$
	declare 
		film_record record;
		film_cursor cursor for
		select i.film_id, count(i.film_id) rent_count from rental r
		join inventory i on i.inventory_id = r.inventory_id
		group by i.film_id;
	begin
		open film_cursor;

		loop
			fetch next from film_cursor into film_record;
			exit when not found;

			if film_record.rent_count < 5 then
				update film 
				set rental_rate = (select rental_rate from film where film_id = film_record.film_id) + 1
				where film_id = film_record.film_id;

				raise notice 'Updated rate for movie : % ', film_record.film_id;
			end if;

		end loop;

		close film_cursor;
	end;
$$

-- Create a function using a cursor that collects titles of all films from a particular category.


select f.film_id, f.title, c.name as category from film_category fc
join film f on f.film_id = fc.film_id
join category c on c.category_id = fc.category_id
order by title

create or replace function select_film_by_category(p_category varchar(50))
returns table (
	film_id int,
	title varchar(100),
	category varchar(50)
) as
$$
	declare
		film_record record;
		film_cursor cursor for
		select f.film_id, f.title, c.name as category from film_category fc
		join film f on f.film_id = fc.film_id
		join category c on c.category_id = fc.category_id;

	begin
		open film_cursor;

		loop
			fetch next from film_cursor into film_record;
			exit when not found;

			if film_record.category = p_category then
				film_id := film_record.film_id;
            	title := film_record.title;
            	category := film_record.category;
            	return next;
				
			end if;
			
		end loop;
		close film_cursor;
	end;
$$ language plpgsql


select * from select_film_by_category('Horror')


-- Loop through all stores and count how many distinct films are available in each store using a cursor.

select * from inventory

select distinct film_id from inventory 

select count(*) from (select distinct film_id, store_id from inventory 
) where store_id = 2


create or replace function count_store_film()
returns table (
	storeId int,
	filmCount int
)
as
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
				
				film_count := film_count+1;
			end loop;
			close inv_cursor;
			storeId := store_record.store_id;
			filmCount := film_count;

			return next;
		end loop;
		close store_cursor;
	end;
$$ language plpgsql;

select * from count_store_film();

-- Write a trigger that logs whenever a new customer is inserted.

create or replace function insert_customer_trigger()
returns trigger as
$$
	begin
		raise notice 'New customer is inserted \n Name: %, Email: %', NEW.first_name, NEW.email;
		return null;
	end;
$$ language plpgsql;


create trigger insert_customer
after insert on customer
for each row
execute function insert_customer_trigger();

select * from customer;

insert into customer (store_id, first_name, last_name, email, address_id, activebool, active) values (1, 'sai', 'prashanth', 'sai@gmail.com', 1,true, 1)

-- Create a trigger that prevents inserting a payment of amount 0

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

insert into payment (customer_id, staff_id, rental_id, amount, payment_date) values (1, 1, 1, 23, now())

-- Set up a trigger to automatically set last_update on the film table before update.

select * from film where film_id = 1;

create or replace function update_film_func()
returns trigger as
$$
	begin
		new.last_update = now();
		return new;
	end
$$language plpgsql

create trigger update_film_trigger
after update on film
for each row
execute function update_film_func()

update film set rental_rate = 1.01 where film_id = 1;

-- Create a trigger to log changes in the inventory table (insert/delete).

create or replace function log_changes()
returns trigger as
$$
	begin
		raise notice 'A record has been %', tg_op;
		return null;
	end
$$ language plpgsql;

create trigger log_changes_trigger
after insert or update or delete on inventory
for each row
execute function log_changes()

select * from inventory

update inventory set film_id = 2 where inventory_id = 1;
update inventory set film_id = 1 where inventory_id = 1;

-- Write a trigger that ensures a rental can’t be made for a customer who owes more than $50.

create or replace function prevent_rent()
returns trigger as
$$
	declare
  		total_paid NUMERIC;
	begin
	  select coalesce(SUM(amount), 0)
	  into total_paid
	  from payment
	  where customer_id = NEW.customer_id;
	
	  if total_paid < 50 then
	    raise notice 'Customer % cannot rent due to outstanding', NEW.customer_id;
		return null;
	  end if;
	
	  return new;
	 end
$$ language plpgsql

create trigger prevent_rent_trigger
before insert on rental
for each row
execute function prevent_rent();

-- Write a transaction that inserts a customer and an initial rental in one atomic operation.

begin;

with new_customer as 
(
	insert into customer (store_id, first_name, last_name, email, address_id, active) values (1, 'sai p', 'rashanth k', 'prs@gmail.com', 1, 1)
	returning customer_id
)

insert into rental (rental_date, inventory_id, customer_id, return_date, staff_id) values (now(), 1, (select customer_id from new_customer), now() + interval '2 days', 1);

commit;

-- Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.   (Ace Goldfinger)

begin;

select * from film where film_id =2;

update film set title = 'Ace Goldfinge' where film_id = 2;

insert into payment (customer_id, staff_id, rental_id, amount, payment_date) values (1, 1, 1, 0, now());

commit;

-- Create a transaction that transfers an inventory item from one store to another.

begin;

select * from inventory where inventory_id = 50;

update inventory
set store_id = 1
where inventory_id = 50;

commit;

-- Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one.

begin;

select * from payment order by payment_id;

update payment
set amount = amount - 10
where payment_id = 17503;

savepoint updated_one;

update payment
set amount = amount - 10
where payment_id = 17503;

rollback to savepoint updated_one;

commit;


-- Write a transaction that deletes a customer and all associated rentals and payments, ensuring atomicity.

call get_overdue_rentals()

begin;

delete from payment where customer_id = 299;

delete from rental where customer_id = 299;

delete from customer where customer_id = 299;

commit;
