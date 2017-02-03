using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Utilities.Common
{
    public class TxScope
    {
        private readonly TransactionScope txscope;

        private bool isComplete;
        public TxScope()
        {
            isComplete = true;
        }
        public TxScope(TransactionScope tx)
        {
            txscope = tx;
            isComplete = true;
        }
        protected void Abort()
        {
            isComplete = false;
        }
        protected void Rollback()
        {
            isComplete = false;
            if (((txscope != null)))
            {
                txscope.Dispose();
            }
        }
        protected void Complete()
        {
            isComplete = isComplete & true;
            if ((isComplete & ((txscope != null))))
            {
                try
                {
                    txscope.Complete();
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    txscope.Dispose();
                }
            }
        }
    }
}
