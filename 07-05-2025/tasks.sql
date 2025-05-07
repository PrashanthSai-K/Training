------------BASIC-------------

-- Retrieve everything from a table

SELECT * FROM cd.facilities

-- Retrieve specific columns from a table

select name, membercost from cd.facilities

-- Control which rows are retrieved

select * from cd.facilities where membercost > 0 and guestcost > 0

-- Control which rows are retrieved - part 2

select facid, name, membercost, monthlymaintenance from cd.facilities where membercost > 0 and membercost < monthlymaintenance / 50

-- Basic string searches

select * from cd.facilities where name like '%Tennis%'

-- Matching against multiple possible values

select * from cd.facilities where facid in (1, 5)

-- Classify results into buckets

select name, case when monthlymaintenance > 100 then 'expensive' else 'cheap' end as cost from cd.facilities

-- Working with dates

select memid, surname, firstname, joindate from cd.members where joindate >= '2012-09-01 00:00:00'

-- Removing duplicates, and ordering results

select distinct surname from cd.members order by surname limit 10;

-- Combining results from multiple queries

select surname from cd.members union select name from cd.facilities

-- Simple aggregation

select joindate from cd.members order by joindate desc limit 1

-- More aggregation

select firstname, surname, joindate from cd.members order by joindate desc limit 1


---------JOINS-------------

-- Retrieve the start times of members' bookings

select starttime from cd.bookings b join cd.members m on b.memid = m.memid where surname = 'Farrell' and firstname = 'David'

-- Work out the start times of bookings for tennis courts

select b.starttime, f.name 
from cd.bookings b 
join cd.facilities f on b.facid = f.facid
where f.name like 'Tennis%' 
and b.starttime between '2012-09-21 00:00:00' AND '2012-09-21 23:59:59'
order by starttime

-- Produce a list of all members who have recommended another member

select distinct m2.firstname,  m2.surname 
from cd.members m1 
inner join cd.members m2 on m2.memid = m1.recommendedby
order by surname, firstname

-- Produce a list of all members, along with their recommender

select m1.firstname as memfname, m1.surname as memsname, m2.firstname as recfname, m2.surname as recsname
from cd.members m1
left outer join cd.members m2
on m2.memid = m1.recommendedby
order by memsname, memfname

-- Produce a list of all members who have used a tennis court

select distinct concat(m.firstname, ' ',m.surname) as member, f.name
from cd.bookings b
join cd.members m on m.memid = b.memid
join cd.facilities f on f.facid = b.facid
where f.name like 'Tennis%'
order by member, f.name

-- Produce a list of costly bookings

select 
	concat(m.firstname, ' ',m.surname) as member, 
	f.name, 
	case
		when m.memid = 0 then
			b.slots * f.guestcost
		else
			b.slots * f.membercost
	end as cost
from cd.bookings b
inner join cd.facilities f on f.facid = b.facid
inner join cd.members m on m.memid = b.memid
where
	b.starttime between '2012-09-14' and '2012-09-15' and
	( (m.memid = 0 and b.slots * f.guestcost > 30) or
	  (m.memid != 0 and b.slots * f.membercost > 30) )
order by cost desc

-- Produce a list of all members, along with their recommender, using no joins.

select 
	distinct concat(m1.firstname, ' ', m1.surname) as member,
	(select 
	 	concat(m2.firstname, ' ', m2.surname) as recommender
	  from cd.members m2
	  where m1.recommendedby = m2.memid
	 )
from
	cd.members m1
order by member

-- Produce a list of costly bookings, using a subquery

select member, facility, cost from
( select 
	concat(m.firstname, ' ',m.surname) as member, 
	f.name  as facility, 
	case
		when m.memid = 0 then
			b.slots * f.guestcost
		else
			b.slots * f.membercost
	end as cost
 from cd.bookings b
 inner join cd.facilities f on f.facid = b.facid
 inner join cd.members m on m.memid = b.memid
 where
	 b.starttime between '2012-09-14' and '2012-09-15'
) as bookings 
where cost > 30
order by cost desc