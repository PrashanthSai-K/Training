/* Set up replica server 

initdb -D "C:/pg_primary"

initdb -D "C:/pg_secondary"

-- To stop already running main server to utilize the port 5432

pgctl -D "C:\Program Files\PostgreSQL\17\data" stop 

pg_ctl -D C:/pg_primary -o "-p 5432" -l C:\pg_primary\logfile start

psql -p 5432 -d postgres -c "CREATE ROLE replicator with REPLICATION LOGIN PASSWORD 'postgres';"

pg_basebackup -D C:/pg_secondary -Fp -Xs -P -R -h 127.0.0.1 -U replicator -p 5433

pg_ctl -D C:/pg_secondary -o "-p 5433" -l d:\sec\logfile start

*/

-- psql -p 5432 -d postgres (login to primary )

 create table rental_log -- create table
 ( 
    log_id serial primary key, 
    rental_time timestamp, 
    customer_id int, 
    film_id, 
    amount numeric, 
    loggen_on timestamp default current_timestamp
 );

-- Create Procedure

create or replace procedure proc_insert_rental_log(p_customer_id int, p_film_id int, p_amount int)
as
$$
    begin
        insert into rental_log (rental_time, customer_id, film_id, amount) values (now(), p_customer_id, p_film_id, p_amount);
    end;
$$ language plpgsql;

-- Call procedure and insert

call proc_insert_rental_log(1, 1, 10);

-- Create log table

create table updated_log (id serial primary key, table_name text, description text, updated_on timestamp);

-- Create trigger function

create or replace function update_rental_trg_func()
returns trigger
as
$$
    declare
        v_table_name text := TG_TABLE_NAME;
    begin
        insert into  updated_log (table_name, description, updated_on) values (v_table_name, 'Updated rental log', now());
    end;
$$ language plpgsql;

-- Create trigger

create trigger trg_update_rental_log
after update on rental_log
for each row
execute function update_rental_trg_func();

-- Check trrigger working

update rental_log set film_id = 10 where log_id = 1;


-- psql -p 5433 -d postgres (login into secondary in another cmd promt)

-- Check table data if replicated properly

select * from rental_log;

-- Check log data if replicated properly
 
select * from updated_log;





