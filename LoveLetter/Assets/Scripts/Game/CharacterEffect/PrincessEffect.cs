
public class PrincessEffect : CharacterEffect
{
    public override CharacterType CharacterType => CharacterType.Princess;

    public override bool DoEffect(PlayerScript player, int cardId)
    {
        Text.ActionSync(player.PlayerName + " discarded the princess - instant lose");
        player.PlayerStatus = PlayerStatus.Intercepted;

        GameManager.instance.CardEffectPlayed(cardId, player);
        return true;
    }
}

