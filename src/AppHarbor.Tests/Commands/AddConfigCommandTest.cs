﻿using AppHarbor.Commands;
using Moq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace AppHarbor.Tests.Commands
{
	public class AddConfigCommandTest
	{
		[Theory, AutoCommandData]
		public void ShouldThrowIfNoArguments(AddConfigCommand command)
		{
			Assert.Throws<CommandException>(() => command.Execute(new string[0]));
		}

		[Theory]
		[InlineAutoCommandData("foo=bar")]
		[InlineAutoCommandData("foo=bar bar=baz")]
		public void ShouldAddConfigurationVariables(string arguments,
			[Frozen]Mock<IApplicationConfiguration> applicationConfiguration,
			[Frozen]Mock<IAppHarborClient> client,
			AddConfigCommand command, string applicationId)
		{
			applicationConfiguration.Setup(x => x.GetApplicationId()).Returns(applicationId);
			var configPairs = arguments.Split();

			command.Execute(configPairs);

			foreach (var configPair in configPairs)
			{
				var splitted = configPair.Split('=');
				client.Verify(x => x.CreateConfigurationVariable(applicationId, splitted[0], splitted[1]));
			}
		}
	}
}
