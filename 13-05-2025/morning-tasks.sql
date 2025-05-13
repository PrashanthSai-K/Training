-- 1. Try two concurrent updates to same row → see lock in action.

-- Transaction 1

begin;

update accounts 
set balance = balance - 100
where id = 3;

commit;

-- Transaction 2

begin;

update accounts 
set balance = balance + 100
where id = 3;

commit;

-- Till transaction 1 commits or rollsback, trasaction 2 will be qeued due to row level locking during updation


-- 2. Write a query using SELECT...FOR UPDATE and check how it locks row.

-- Transaction 1

begin;

select * from accounts where id = 3 for update;

commit;

-- Transaction 2

begin;

update accounts 
set balance = balance + 100 -- Will be qeued till transaction 1 commits or rollback, because transaction 1 locked row by select ... for update
where id = 3;

commit;

-- 3. Intentionally create a deadlock and observe PostgreSQL cancel one transaction.

-- Transaction 1

begin;

select * from accounts where id = 3 for update;

select * from accounts where id = 4 for update;

commit;

-- Transaction 2 

begin;

select * from accounts where id = 4 for update;

select * from accounts where id = 3 for update;

commit;

-- This creates a deadlock situation, so postgresql detected and auto cancelled transaction 2

/*
ERROR:  deadlock detected
Process 17420 waits for ShareLock on transaction 1052; blocked by process 19872.
Process 19872 waits for ShareLock on transaction 1060; blocked by process 17420. 

SQL state: 40P01
Detail: Process 17420 waits for ShareLock on transaction 1052; blocked by process 19872.
Process 19872 waits for ShareLock on transaction 1060; blocked by process 17420.
Hint: See server log for query details.
Context: while locking tuple (0,23) in relation "accounts"
*/

-- 4. Use pg_locks query to monitor active locks.

SELECT query,usename, application_name, datname, mode, database, locktype from pg_locks l
JOIN pg_stat_activity a ON l.pid = a.pid;

-- This will show a list of active locks

-- 5. Explore about Lock Modes.

begin;

lock table accounts
in access share mode;

commit;

begin

lock table accounts
in row share mode;

commit;

begin;

lock table accounts 
in exclusive mode;

commit;

begin;

lock table aacounts
in access exclusive mode;

commit;


