
public class SpyEffect : CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Spy;

    public override bool CanDoEffect(PlayerScript player, int cardId) => true;
    public override bool DoEffect(PlayerScript player, int cardId)
    {
        Textt.ActionSync(player.PlayerName + " played a spy");
        GameManager.instance.CardEffectPlayed(cardId, player.PlayerId);
        return true;
    }
}

