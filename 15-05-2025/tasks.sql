
create table customers (
	id serial primary key,
	name text, 
	email text
);


create extension if not exists pgcrypto;

-- 1. Create a stored procedure to encrypt a given text

create or replace procedure sp_encrypt_data(p_data text, out p_encrypted text)
as
$$
	begin
		 p_encrypted := encode(pgp_sym_encrypt(p_data, 'abcd1234567890efgh'::text), 'base64');
	end;
$$ language plpgsql;

call sp_encrypt_data('sample_data', null)

-- 2. Create a stored procedure to compare two encrypted texts

create or replace procedure sp_compare_encrypted(p_encrypted_1 text, p_encrypted_2 text, out res boolean)
as
$$
declare
	v_data1 text;
	v_data2 text;
begin
	v_data1 := pgp_sym_decrypt(decode(p_encrypted_1, 'base64'), 'abcd1234567890efgh'::text);
	v_data2 := pgp_sym_decrypt(decode(p_encrypted_2, 'base64'), 'abcd1234567890efgh'::text);
	if v_data1 = v_data2 then
		res := true;
	else
		res := false;
	end if;
end;	
$$ language plpgsql;

call sp_compare_encrypted('ww0EBwMCX4VAf9Am1cly0jwBl/suNC+WvYI7ArSmq+H+xX5YDNb21+8qOox3USi6lVTE8UX7rPHC
ShKjRPgzjmgvtgce3qTHtpLqr5A=', 'ww0EBwMCX4VAf9Am1cly0jwBl/suNC+WvYI7ArSmq+H+xX5YDNb21+8qOox3USi6lVTE8UX7rPHC
ShKjRPgzjmgvtgce3qTHtpLqr5A=', null)

-- 3. Create a stored procedure to partially mask a given text

create or replace procedure sp_mask_data(p_data text, out p_result text)
as
$$
	begin
		if length(p_data) <= 4 then
			p_result := p_data;
		else
			p_result := concat(left(p_data, 2), repeat('*', length(p_data) - 4), right(p_data, 2));
		end if;
	end;
$$ language plpgsql;

call sp_mask_data('sai@gmail.com', null)

-- 4. Create a procedure to insert into customer with encrypted email and masked name

create or replace procedure sp_insert_customer(p_name text, p_email text)
as
$$
	declare
		v_msk_name text;
		v_enc_email text;
	begin
		call sp_mask_data(p_name, v_msk_name);
		call sp_encrypt_data(p_email, v_enc_email);

		insert into customers (name, email) values (v_msk_name, v_enc_email);
	end;
$$ language plpgsql;

call sp_insert_customer('hariharan p', 'hari@gmail.com');

-- 5. Create a procedure to fetch and display masked first_name and decrypted email for all customers

create or replace procedure sp_show_customers()
as
$$
	declare
		cus_rec record;
		cus_cur cursor for
		select * from customers;
		customer_id int;
		customer_name text;
		customer_email text;
	begin
		open cus_cur;

		loop
			fetch next from cus_cur into cus_rec;
			exit when not found;
			
			customer_id := cus_rec.id;
			customer_name := cus_rec.name;
			customer_email := pgp_sym_decrypt(decode(cus_rec.email, 'base64'), 'abcd1234567890efgh'::text);
			
			raise notice 'Customer ID : %, Customer Name : %, Customer Email : %', customer_id, customer_name, customer_email;
			
		end loop;
		
		close cus_cur;
	end;	
$$ language plpgsql;

select * from customers;
call sp_show_customers();
