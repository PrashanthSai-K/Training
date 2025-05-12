create table accounts(
	id serial primary key,
	name varchar(200),
	balance numeric(10, 2)
)

select * from accounts;
-- sample transaction

begin transaction;

insert into accounts (name, balance)
values ('sai', 4000),
		('hari', 5000);

commit;

-- Transaction as per Image shared (Morning session)

begin transaction;

update accounts
set balance = balance - 500
where id = 3;

savepoint s1;

update accounts
set balance = balance + 500
where id = 4;

commit;

-- Concurrency - using MVCC(Multi-Version Concurrency Control)

-- Read When someone is updating

-- T1
begin;

update accounts set balance = balance - 200 where id = 4;

-- T2
begin;

select * from accounts; -- Last commited value read

/*
Concurrency -Isolation levels :

	1. READ UNCOMMITTED -> PSQL not supported
	2. READ COMMITTED   -> Default and uses MVCC
	3. REPEATABLE READ  -> Ensures repeatable reads
	4. SERIALIZABLE     -> Full isolation (safe but slow and performance issues)

Problems without concurrency control :

	1. Inconsistent reads (reading uncommited data, non-repeatable reads, phantom read)
		- dirty read : reading uncommited data
		- non-repeatable read :  when u read some data and someone updates it, after sometime u read same data and get different value
		- phantom read : based on a condition you filtered some rows and after some time when you try to filter the same 
						 and get different rows because of newly inserted row matching same filter params
	____________________________________________________________________
	|Isolation level  | Dirty Read | Non-Repeatable Read | Phantom Read |
	--------------------------------------------------------------------
	|Read Uncommitted |    ✅      |         ✅          |      ✅     |
	--------------------------------------------------------------------
	|Read Committed   |    ❌      |         ✅          |      ✅     |
	--------------------------------------------------------------------
	|Repeatable Read  |    ❌      |         ❌          |      ✅     |
	--------------------------------------------------------------------
	|Serializable     |    ❌      |         ❌          |      ❌     |
	--------------------------------------------------------------------
	❌ - Prevented anomaly
	✅ - Possible anomaly
		
	2. Lost update (trans a read a value , the trans b read and update a value, now at last a updates value based on he read. so trans b is lost)

	To avoid lost updates :

		1. Pessimistic locking - lock when someone read and dont unlock till all transaction completes (prevents concurrency by blocking)
		2. Optimistic locking - use version numbering while reading and based on that update, if version number mismatches cancel update and read again (fast)
		3. Using Serialization Isolation level 
*/

-- Phantom read example

-- Trans 1
begin;

select * from accounts
where balance > 500;
-- 2 rows

-- Trans 2
begin;

insert into Accounts
(id, balance)
values
(2, 600);

commit;

-- Trans 1
select * from Accounts
where balance > 500;
-- 3 rows
-- Earlier it was 2 now it is 3. Voilaa!! A phatom new row appeared!


