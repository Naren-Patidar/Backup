----Ranks Example
--ROW_NUMBER () OVER ([<partition_by_clause>] <order_by_clause>)
--Returns the sequential number of a row within a partition of a result set, starting at 1 for the first row in each partition.

--RANK () OVER ([<partition_by_clause>] <order_by_clause>)
--Returns the rank of each row within the partition of a result set.

--DENSE_RANK () OVER ([<partition_by_clause>] <order_by_clause>)
--Returns the rank of rows within the partition of a result set, without any gaps in the ranking.

--NTILE (integer_expression) OVER ([<partition_by_clause>] <order_by_clause>)
--Distributes the rows in an ordered partition into a specified number of groups.

CREATE TABLE #StudentDetails
(
StudentID INTEGER PRIMARY KEY,
StudentName VARCHAR(15),
Marks INT,
Section Char(2)
)

INSERT INTO #StudentDetails
Select 1,'SMITH',20,'A'
UNION ALL 
Select 2,'B',29,'A'
UNION ALL 
Select 3,'A',29,'B'
UNION ALL 
Select 4,'C',29,'A'
UNION ALL 
Select 5,'D',30,'B'
UNION ALL 
Select 6,'E',30,'A'
UNION ALL 
Select 7,'F',40,'A'
UNION ALL 
Select 8,'G',29,'B'
UNION ALL 
Select 9,'H',40,'B'
UNION ALL 
Select 10,'I',40,'B'
UNION ALL 
Select 11,'J',41,'A'

Select * from  #StudentDetails

Select StudentID,StudentName,Marks,Section, ROW_NUMBER() OVER( Partition by Section Order by Marks) as RowNumber from #StudentDetails    
Select StudentID,StudentName,Marks,Section, RANK() OVER(Partition by Section Order by Marks) as RankE from #StudentDetails 
Select StudentID,StudentName,Marks,Section, DENSE_RANK() OVER(Partition by Section Order by Marks desc) as DenseRank from #StudentDetails 
Select StudentID,StudentName,Marks,Section, NTILE(3) OVER(Partition by Section Order by Marks) as NtileE from #StudentDetails 

Select StudentID,StudentName,Marks
,ROW_NUMBER() OVER(Order by Marks) as RowNumber
,RANK() OVER(Order by Marks) as RankE
,DENSE_RANK() OVER(Order by Marks) as DenseRank
, NTILE(5) OVER(Order by Marks) as NtileE from #StudentDetails 

Drop table #StudentDetails 