using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.DataAccessLayer.Helpers
{
	public static class DbContextHelper
	{
		public static void ClearSqlitePool(DbContext dbContext)
		{
			SqliteConnection.ClearPool((SqliteConnection) dbContext.Database.GetDbConnection());
		}
	}
}
