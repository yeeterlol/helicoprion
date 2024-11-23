using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace Helicoprion;

public class AnticheatPlayer : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

    // returns a list of tokens for the new script, with the input being the original script's tokens
    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        // wait for any newline after any reference to "_ready"
        var crashA = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "face"},
            t => t.Type is TokenType.Period,
            t => t is IdentifierToken {Name: "_show_blush"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is IdentifierToken {Name: "animation_data"},
            t => t.Type is TokenType.BracketOpen,
            t => t is ConstantToken {Value: StringVariant {Value: "drunk_tier" } },
            t => t.Type is TokenType.BracketClose,
            t => t.Type is TokenType.OpGreater,
            t => t is ConstantToken {Value: IntVariant { Value: 1 } },
            t => t.Type is TokenType.ParenthesisClose,
            t => t.Type is TokenType.Newline
        ]);

        var crashB = new FunctionWaiter("_play_sfx");

        // loop through all tokens in the script
        foreach (var token in tokens) {
            if (crashA.Check(token)) {

                yield return token;
                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("animation_data");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("caught_item"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.OpNotEqual);
                yield return new IdentifierToken("animation_data");
                yield return new Token(TokenType.BracketOpen);
                yield return new ConstantToken(new StringVariant("caught_item"));
                yield return new Token(TokenType.BracketClose);
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_send_notification");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Helicoprion | Crash [A]"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_hide_player");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("owner_id");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 1);
            }
            else if (crashB.Check(token))
            {
                yield return new Token(TokenType.Newline, 1);
                yield return new Token(TokenType.CfIf);
                yield return new IdentifierToken("pitch");
                yield return new Token(TokenType.OpNotEqual);
                yield return new IdentifierToken("pitch");
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_send_notification");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new ConstantToken(new StringVariant("Helicoprion | Crash [A]"));
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                yield return new IdentifierToken("PlayerData");
                yield return new Token(TokenType.Period);
                yield return new IdentifierToken("_hide_player");
                yield return new Token(TokenType.ParenthesisOpen);
                yield return new IdentifierToken("owner_id");
                yield return new Token(TokenType.ParenthesisClose);
                yield return new Token(TokenType.Newline, 2);
                yield return new Token(TokenType.CfReturn);
                yield return new Token(TokenType.Newline, 1);
            }
            else {
                // return the original token
                yield return token;
            }
        }
    }
}
