namespace Spectre.Console.Tests.Unit;

public sealed class SelectionPromptTests
{
    [Fact]
    public void Should_Not_Throw_When_Selecting_An_Item_With_Escaped_Markup()
    {
        // Given
        var console = new TestConsole();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);
        var input = "[red]This text will never be red[/]".EscapeMarkup();

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices(input);
        prompt.Show(console);

        // Then
        console.Output.ShouldContain(@"[red]This text will never be red[/]");
    }

    [Fact]
    public void Should_Render_Correctly()
    {
        // Given
        var console = new TestConsole()
            .Width(80)
            .Interactive()
            .EmitAnsiSequences();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices("Choice 1", "Choice 2");
        prompt.Show(console);

        // Then
        console.Output
            .NormalizeLineEndings()
            .ShouldBe(
                "[?25l" + // Hide cursor
                "Select one \n" + // Prompt
                "           \n" +
                "[38;5;12m> Choice 1[0m \n" +
                "  Choice 2 [3ASelect one \n" +
                "           \n" +
                "[38;5;12m> Choice 1[0m \n" +
                "  Choice 2 " +
                "[2K[1A[2K[1A[2K[1A[2K[?25h"); // Clear + show cursor
    }

    [Fact]
    public void Should_Show_Result_If_Set()
    {
        // Given
        var console = new TestConsole()
            .Width(80)
            .Interactive()
            .EmitAnsiSequences();
        console.Profile.Capabilities.Interactive = true;
        console.Input.PushKey(ConsoleKey.Enter);

        // When
        var prompt = new SelectionPrompt<string>()
                .Title("Select one")
                .AddChoices("Choice 1", "Choice 2")
                .ShowResult(true);
        prompt.Show(console);

        // Then
        console.Output
            .NormalizeLineEndings()
            .ShouldBe(
                "[?25l" + // Hide cursor
                "Select one \n" + // Prompt
                "           \n" +
                "[38;5;12m> Choice 1[0m \n" +
                "  Choice 2 [3ASelect one \n" +
                "           \n" +
                "[38;5;12m> Choice 1[0m \n" +
                "  Choice 2 " +
                "[2K[1A[2K[1A[2K[1A[2K" + // Clear
                "Select one [38;5;12mChoice 1[0m" + // Show result
                "[?25h"); // Show cursor
    }
}
