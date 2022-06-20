
public class SpyEffect : ICharacterEffect
{
    public CharacterType CharacterType => CharacterType.Spy;

    public bool DoEffect(PlayerScript player, int cardId)
    {
        MonoHelper.Instance.SetActionText(player.PlayerName + " played a spy");
        GameManager.instance.CardEffectPlayed(cardId, player);
        return true;
    }
}

