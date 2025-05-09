select * from actor

select * from film

select * from film_actor

-- List all films with their length and rental rate, sorted by length descending.

select title as film_name, length, rental_rate from film order by length desc

-- Find the top 5 customers who have rented the most films.

select rental.customer_id, customer.first_name ,count(rental.customer_id) as films from rental 
join customer on customer.customer_id = rental.customer_id
group by rental.customer_id, customer.first_name
limit 5

-- Display all films that have never been rented.

select 
	f.film_id, 
	f.title 
from film f
left outer join inventory i on i.film_id = f.film_id
where i.inventory_id is null
order by f.title 


-- List all actors who appeared in the film ‘Academy Dinosaur’.

select 
	f.film_id, 
	f.title, 
	a.actor_id, 
	concat(a.first_name,' ',a.last_name) as actor_name 
from film_actor fa
join film f on f.film_id = fa.film_id
join actor a on a.actor_id = fa.actor_id
where f.title = 'Academy Dinosaur'

-- List each customer along with the total number of rentals they made and the total amount paid.

select 
	r.customer_id,
	concat(c.first_name, ' ', c.last_name) as customer_name,
	count(r.customer_id) total_rentals, 
	sum(p.amount) amount_paid
from rental r
join payment p on p.rental_id = r.rental_id
join customer c on c.customer_id = r.customer_id
group by r.customer_id, c.first_name,  c.last_name

-- Using a CTE, show the top 3 rented movies by number of rentals.

select * from rental
select * from film
select * from inventory

with cte_movie_count as 
(
	select 
		f.title,
		count(r.rental_id) as rental_count
	from film f
	join inventory i on i.film_id = f.film_id
	join rental r on r.inventory_id = i.inventory_id
	group by f.title
)

select * from cte_movie_count order by rental_count desc limit 3

-- Find customers who have rented more than the average number of films.

with cte_CountRental as
(
	select 
		c.customer_id,
		concat(c.first_name, ' ', c.last_name) as customer_name,
		count(r.customer_id) as mov_count
	from rental r
	join customer c on c.customer_id = r.customer_id
	group by c.customer_id, c.first_name,  c.last_name
),cte_AvgRental as
(
	select avg(mov_count) as avg_rental from cte_CountRental
)

select customer_id, customer_name, mov_count from cte_CountRental
where mov_count > (select avg_rental from  cte_AvgRental)
limit 3

-- Write a function that returns the total number of rentals for a given customer ID.

create or replace function get_total_rentals(p_customer_id int)
returns int
as
$$
	declare total_rentals int;
	begin
		select 
			count(r.customer_id) into total_rentals
		from rental r
		where r.customer_id = p_customer_id;
		return total_rentals;
	end;
$$ language plpgsql

select 
	customer_id, 		
	concat(customer.first_name, ' ', customer.last_name) as customer_name,
	get_total_rentals(customer_id) as rental_count 
from customer

-- Write a stored procedure that updates the rental rate of a film by film ID and new rate.

create procedure update_rental_rate(p_film_id int, p_rental_rate float)
as
$$
begin
	update film set rental_rate = p_rental_rate where film_id = p_film_id;
end
$$ language plpgsql;

call update_rental_rate(1, 1.56)

update film set rental_rate = 1.12 where film_id = 1

select * from film order by film_id

-- Write a procedure to list overdue rentals (return date is NULL and rental date older than 7 days).

create or replace procedure get_overdue_rentals()
as
$$
declare rec record;
begin
	for rec in
		select 
			*,
			(return_date - rental_date) as due_by 
		from rental 
		where 
			return_date is null 
			or
			(return_date - rental_date > INTERVAL'7 days' )
	loop 
        raise notice 'Rental ID: %, Customer ID: %, Rental Date: %, Return Date: %, Due By: %',
            rec.rental_id, rec.customer_id, rec.rental_date, rec.return_date, rec.due_by;
	end loop;
end
$$ language plpgsql

call get_overdue_rentals()

