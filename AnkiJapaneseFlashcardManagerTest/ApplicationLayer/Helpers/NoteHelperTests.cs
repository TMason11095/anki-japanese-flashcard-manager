using AnkiJapaneseFlashcardManager.ApplicationLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests.ApplicationLayer.Helpers
{
	public class NoteHelperTests
	{
		[Theory]
		//Test Case: Ids found
		[InlineData(new string[] { "kid:1474", "nl:N4", "skid:1472", "skid:466" }, "kid:", new string[] { "1474" })]
		[InlineData(new string[] { "kid:1474", "nl:N4", "skid:1472", "skid:466" }, "skid:", new string[] { "1472", "466" })]
		//Test Case: No ids found
		[InlineData(new string[] { "kid:1474", "nl:N4", "skid:1472", "skid:466" }, "rng:", new string[] { })]
		//Test Case: No tag list to search from
		[InlineData(new string[] { }, "rng:", new string[] { })]
		//Test Case: No tag given
		[InlineData(new string[] { "kid:1474", "nl:N4", "skid:1472", "skid:466" }, "", new string[] { "kid:1474", "nl:N4", "skid:1472", "skid:466" })]
		[InlineData(new string[] { }, "", new string[] { })]
		public void Get_ids_from_tag_list(string[] tagList, string tag, string[] expectedIds)
		{
			//Act
			var ids = NoteHelper.GetIdsFromTagList(tagList, tag);

			//Assert
			ids.Should().BeEquivalentTo(expectedIds);
		}
	}
}
