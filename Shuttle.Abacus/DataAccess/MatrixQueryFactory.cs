using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Data;

namespace Shuttle.Abacus.DataAccess
{
    public class MatrixQueryFactory : IMatrixQueryFactory
    {
        private const string MatrixQuery = @"
select {0}
    m.Id,
    m.Name,
    m.RowArgumentId,
    ra.Name RowArgumentName,
    m.ColumnArgumentId,
    ca.Name ColumnArgumentName,
    m.DataTypeName
from
    Matrix m
inner join
    Argument ra on ra.Id = m.RowArgumentId
left join
    Argument ca on ca.Id = m.ColumnArgumentId
where
(
    @Id is null
    or
    m.Id = @Id
)
and
(
    @Name is null
    or
    @Name = ''
    or
    m.Name like '%' + @Name + '%'
)
order by 
    m.Name
";

        public IQuery All()
        {
            return RawQuery.Create(string.Format(MatrixQuery, string.Empty))
                .AddParameterValue(Columns.Id, null)
                .AddParameterValue(Columns.Name, null);
        }

        public IQuery Get(Guid id)
        {
            return RawQuery.Create(@"
select
    Id,
    Name,
    ColumnArgumentId,
    RowArgumentId,
    DataTypeName
from
    Matrix
where
    Id = @Id
")
                .AddParameterValue(Columns.Id, id);
        }

        public IQuery Add(Guid id, string name, Guid? columnArgumentId, Guid rowArgumentId, string dataTypeName)
        {
            return RawQuery.Create(@"
insert into Matrix
(
    Id,
    Name,
    ColumnArgumentId,
    RowArgumentId,
    DataTypeName
)
values
(
    @Id,
    @Name,
    @ColumnArgumentId,
    @RowArgumentId,
    @DataTypeName
)")
                .AddParameterValue(Columns.Id, id)
                .AddParameterValue(Columns.Name, name)
                .AddParameterValue(Columns.ColumnArgumentId, columnArgumentId)
                .AddParameterValue(Columns.RowArgumentId, rowArgumentId)
                .AddParameterValue(Columns.DataTypeName, dataTypeName);
        }

        public IQuery Remove(Guid id)
        {
            return
                RawQuery.Create("delete from Matrix where Id = @Id")
                    .AddParameterValue(Columns.Id, id);
        }

        public IQuery RemoveElements(Guid id)
        {
            return
                RawQuery.Create("delete from MatrixElement where MatrixId = @Id")
                    .AddParameterValue(Columns.Id, id);
        }

        public IQuery RemoveConstraints(Guid id)
        {
            return
                RawQuery.Create("delete from MatrixConstraint where MatrixId = @Id")
                    .AddParameterValue(Columns.Id, id);
        }

        public IQuery ConstraintRegistered(Guid matrixId, string axis, int index, Guid id, string comparison,
            string value)
        {
            return RawQuery.Create(@"
if exists
(
    select 
        null
    from
        MatrixConstraint
    where
        MatrixId = @MatrixId
    and
        Axis = @Axis
    and
        [Index] = @Index
)
    update
        MatrixConstraint
    set
        Comparison = @Comparison,
        Value = @Value
    where
        MatrixId = @MatrixId
    and
        Axis = @Axis
    and
        [Index] = @Index
else
    insert into MatrixConstraint
    (
        MatrixId,
        Axis,
        [Index],
        Id,
        Comparison,
        Value
    )
    values
    (
        @MatrixId,
        @Axis,
        @Index,
        @Id,
        @Comparison,
        @Value
    )
")
                .AddParameterValue(Columns.MatrixId, matrixId)
                .AddParameterValue(Columns.Axis, axis)
                .AddParameterValue(Columns.Index, index)
                .AddParameterValue(Columns.Id, id)
                .AddParameterValue(Columns.Comparison, comparison)
                .AddParameterValue(Columns.Value, value);
        }

        public IQuery ElementRegistered(Guid matrixId, int column, int row, Guid id, string value)
        {
            return RawQuery.Create(@"
if exists
(
    select 
        null
    from
        MatrixElement
    where
        MatrixId = @MatrixId
    and
        [Column] = @Column
    and
        [Row] = @Row
)
    update
        MatrixElement
    set
        Value = @Value
    where
        MatrixId = @MatrixId
    and
        [Column] = @Column
    and
        [Row] = @Row
else
    insert into MatrixElement
    (
        MatrixId,
        [Column],
        [Row],
        Id,
        Value
    )
    values
    (
        @MatrixId,
        @Column,
        @Row,
        @Id,
        @Value
    )
")
                .AddParameterValue(Columns.MatrixId, matrixId)
                .AddParameterValue(Columns.Column, column)
                .AddParameterValue(Columns.Row, row)
                .AddParameterValue(Columns.Id, id)
                .AddParameterValue(Columns.Value, value);
        }

        public IQuery Search(MatrixSearchSpecification specification)
        {
            Guard.AgainstNull(specification, nameof(specification));

            return new RawQuery(string.Format(MatrixQuery, string.Empty))
                .AddParameterValue(Columns.Id, specification.Id)
                .AddParameterValue(Columns.Name, specification.Name);
        }

        public IQuery Constraints(Guid id)
        {
            return RawQuery.Create(@"
select
    Id,
    MatrixId,
    Axis,
    [Index],
    Comparison,
    Value
from
    MatrixConstraint
where
    MatrixId = @Id
").AddParameterValue(Columns.Id, id);
        }

        public IQuery Elements(Guid id)
        {
            return RawQuery.Create(@"
select
    Id,
    MatrixId,
    [Column],
    [Row],
    Value
from
    MatrixElement
where
    MatrixId = @Id
").AddParameterValue(Columns.Id, id);
        }

        public IQuery Find(MatrixSearchSpecification specification)
        {
            Guard.AgainstNull(specification, nameof(specification));

            return new RawQuery(string.Format(MatrixQuery, "top 1"))
                .AddParameterValue(Columns.Id, specification.Id)
                .AddParameterValue(Columns.Name, specification.Name);
        }
    }
}