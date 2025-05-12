/*
1️⃣ Question:
In a transaction, if I perform multiple updates and an error happens in the third statement, but I have not used SAVEPOINT, what will happen if I issue a ROLLBACK?
Will my first two updates persist?
*/

begin;

select * from accounts;

update accounts set balance = balance * 2 where id = 3;

update accounts set balance = balance / 2 where id = 4;

update accounta set balance = balance * 3 where id = 3; -- Error occurs here due to table name mismatch

rollback; -- If rollback used then, no updates will ne presisted;

/*
2️⃣ Question:
Suppose Transaction A updates Alice’s balance but does not commit. Can Transaction B read the new balance if the isolation level is set to READ COMMITTED?
*/

-- No in READ COMMITTED isolation level only commited data can be read. uncommited data cannot be read;

-- t1
begin;

update accounts set balance = balance - 200 where id = 3;

-- t2

select * from accounts; -- last commited data will only be read

/*
3️⃣ Question:
What will happen if two concurrent transactions both execute:
UPDATE tbl_bank_accounts SET balance = balance - 100 WHERE account_name = 'Alice';
at the same time? Will one overwrite the other?
*/

-- Postgresql MVCC will prevent this from happening
-- When transaction one is updating a row, that row will be locked till update completes
-- After lock is released, transaction two can update.

/*
4️⃣ Question:
If I issue ROLLBACK TO SAVEPOINT after_alice;, will it only undo changes made after the savepoint or everything?
*/

-- After the savepoint whatever is executed, it will be undone due to rollback to savepoint.
-- Not everything will be undone.

begin;

select * from accounts;

update accounts set balance = balance * 2 where id = 3;

savepoint s1;

update accounts set balance = balance / 2 where id = 4;

rollback to s1; -- will undo update 2 alone

commit;

/*
5️⃣ Question:
Which isolation level in PostgreSQL prevents phantom reads?
*/

-- Serializable isolation level will prevent phantom reads due to full isolation of the database


BEGIN ISOLATION LEVEL SERIALIZABLE;

-- T1
SELECT * FROM accounts WHERE balance > 500;

-- T2 : T2 may not happhen till T1 comlpetes

BEGIN TRANSACTION;

INSERT INTO accounts (name, balance)
VALUES ('kavin', 3000);

COMMIT;


/*
6️⃣ Question:
Can Postgres perform a dirty read (reading uncommitted data from another transaction)?
*/

-- No, READ UNCOMMITTED isolation level is not supported in postgresql.
-- So, it is not possible to read the uncommitted data.

/*
7️⃣ Question:
If autocommit is ON (default in Postgres), and I execute an UPDATE, is it safe to assume the change is immediately committed?
*/

-- Yes, without begin block if any update is executed, it will be committed immediately by postgres


/*
8️⃣ Question:
If I do this:

BEGIN;
UPDATE accounts SET balance = balance - 500 WHERE id = 1;
-- (No COMMIT yet)
And from another session, I run:

SELECT balance FROM accounts WHERE id = 1;
Will the second session see the deducted balance?
*/

-- No, by default READ COMMITTED is the isolation level used by postgres;
-- So, the uncommitted update of deducting balance cannot be seen by any other transaction until or unless it is commited;


-- Transaction Simulation as per Image shared (Morning session)

begin transaction;

update accounts
set balance = balance - 500
where id = 3;

update accounts
set balance = balance + 500
where id = 4;

savepoint s1;

update accounts
set balance = balance + 500
where id = 3;

update accounts
set balance = balance - 500
where id = 4;

commit;
