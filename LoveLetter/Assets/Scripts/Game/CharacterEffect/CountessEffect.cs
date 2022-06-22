
public class CountessEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Countess;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        Text.ActionSync("Countess played, no special effect");
        GameManager.instance.CardEffectPlayed(cardId, player);
        return true;
    }
}

