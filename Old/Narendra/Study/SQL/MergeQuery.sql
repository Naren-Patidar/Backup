-- Merge example
CREATE TABLE StudentDetails
(
StudentID INTEGER PRIMARY KEY,
StudentName VARCHAR(15)
)
GO
INSERT INTO StudentDetails
VALUES(1,'SMITH')
INSERT INTO StudentDetails
VALUES(2,'ALLEN')
INSERT INTO StudentDetails
VALUES(3,'JONES')
INSERT INTO StudentDetails
VALUES(4,'MARTIN')
INSERT INTO StudentDetails
VALUES(5,'JAMES')
GO

CREATE TABLE StudentTotalMarks
(
StudentID INTEGER REFERENCES StudentDetails,
StudentMarks INTEGER
)
GO
INSERT INTO StudentTotalMarks
VALUES(1,230)
INSERT INTO StudentTotalMarks
VALUES(2,255)
INSERT INTO StudentTotalMarks
VALUES(3,200)
GO

Select * from StudentDetails
Select * from StudentTotalMarks


MERGE StudentTotalMarks AS stm
USING (SELECT StudentID,StudentName FROM StudentDetails) AS sd
ON stm.StudentID = sd.StudentID
WHEN MATCHED AND stm.StudentMarks > 250 THEN DELETE
WHEN MATCHED THEN UPDATE SET stm.StudentMarks = stm.StudentMarks + 25
WHEN NOT MATCHED THEN
INSERT(StudentID,StudentMarks)
VALUES(sd.StudentID,25);
GO


Drop table StudentDetails 
Drop table StudentTotalMarks
 