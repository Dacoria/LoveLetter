
public class CountessEffect : CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Countess;

    public override bool DoEffect(PlayerScript player, int cardId)
    {
        Textt.ActionSync("Countess played, no special effect");
        GameManager.instance.CardEffectPlayed(cardId, player.PlayerId);
        return true;
    }
}

