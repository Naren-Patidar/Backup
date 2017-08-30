using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Tesco.Marketing.IT.ClubcardCoupon.DAL
{
    public sealed class AlphaCodeRowNumberGenerator
    {
        private static volatile AlphaCodeRowNumberGenerator instance;
        private static object syncRoot = new Object();
        private long _nextId = 0;
        private long _maxIdWithinBatch = 0;
        private const int _batchSize = 5000;
        private Database clubcardCouponDb = null;
        private int _totalRowsInTable;

        private AlphaCodeRowNumberGenerator()
        {
            clubcardCouponDb = DatabaseFactory.CreateDatabase("OLTPDbServer");
        }

        public static AlphaCodeRowNumberGenerator Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AlphaCodeRowNumberGenerator();
                    }
                }

                return instance;
            }
        }

        public int GetNextRowNumber()
        {
            lock (syncRoot)
            {
                if (_nextId == _maxIdWithinBatch)
                {
                    AllocateNextBatch();
                }

                return (int)(_nextId++ % _totalRowsInTable) + 1;
            }
        }

        private void AllocateNextBatch()
        {
            using (DbCommand cmd = clubcardCouponDb.GetStoredProcCommand("dbo.USP_GetNextAlphacodeSequenceNumbers"))
            {
                cmd.CommandTimeout = 5;

                clubcardCouponDb.AddInParameter(cmd, "@NumberOfAlphacodesToAllocate", DbType.Int32, _batchSize);
                clubcardCouponDb.AddOutParameter(cmd, "@NextSequenceNummber", DbType.Int64, 8);
                clubcardCouponDb.AddOutParameter(cmd, "@TotalRowsInTable", DbType.Int32, 4);

                clubcardCouponDb.ExecuteNonQuery(cmd);

                _nextId = Convert.ToInt64(cmd.Parameters["@NextSequenceNummber"].Value);
                _maxIdWithinBatch = _nextId + _batchSize;
                _totalRowsInTable = Convert.ToInt32(cmd.Parameters["@TotalRowsInTable"].Value);
            }
        }
    }
}
