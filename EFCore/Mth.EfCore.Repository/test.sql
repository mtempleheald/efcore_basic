-- SELECT Name from sys.Databases
-- GO

SELECT Name from MthEfDb.dbo.Children
GO

-- CREATE VIEW dbo.CountChildrenByParent AS
--   SELECT p.Id AS ParentID
--   ,      p.Name AS ParentName
--   ,      COUNT(1) AS CountChildren
--   FROM      MthEfDb.dbo.Parents p
--   LEFT JOIN MthEfDb.dbo.Children c ON  p.Id = c.ParentId
--   GROUP BY p.Id, p.Name;
