
public class PrincessEffect : CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Princess;

    public override bool CanDoEffect(PlayerScript player, int cardId) => true;
    
    public override bool DoEffect(PlayerScript player, int cardId)
    {
        Textt.ActionSync(player.PlayerName + " discarded the princess - instant lose");
        player.PlayerStatus = PlayerStatus.Intercepted;

        GameManager.instance.CardEffectPlayed(cardId, player.PlayerId);
        return true;
    }
}

