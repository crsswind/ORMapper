using ObjectRelationalMapper;

namespace TestBed
{
    [DbTable("Part")]
    public class PartDto
    {
        #region Properties

        [OrmKey]
        [DbColumn("Id")]
        public int PartId { get; set; }

        [DbColumn("Name")]
        public string PartName { get; set; }

        #endregion
    }
}