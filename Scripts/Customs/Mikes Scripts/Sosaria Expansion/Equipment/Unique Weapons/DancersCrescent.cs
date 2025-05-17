using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class DancersCrescent : Scimitar
{
    [Constructable]
    public DancersCrescent()
    {
        Name = "Dancer’s Crescent";
        Hue = Utility.Random(1200, 1300);  // A subtle blend of silver and pale lavender, representing grace and elegance
        MinDamage = Utility.RandomMinMax(25, 45);
        MaxDamage = Utility.RandomMinMax(50, 75);
        Attributes.WeaponSpeed = 10;  // Perfect for fluid, rapid strikes
        Attributes.Luck = 20;  // Enhanced luck for the wielder, as if favored by fate
        Attributes.DefendChance = 15;  // Increased chance to dodge or parry attacks, fitting for a nimble dancer
        
        // Skill bonuses related to agility, movement, and fluidity
        SkillBonuses.SetValues(0, SkillName.Tactics, 10.0);  // Boosts tactical awareness in battle
        SkillBonuses.SetValues(1, SkillName.Swords, 15.0);  // Increases skill with swords, emphasizing mastery with scimitars


        // Thematic bonus: magical and physical prowess for graceful combat
        WeaponAttributes.HitLeechHits = 15;  // Drains life on hit, representing the graceful yet deadly nature of the blade
        WeaponAttributes.HitLeechStam = 10;  // Drains stamina to keep opponents from outlasting the wielder
        WeaponAttributes.BattleLust = 10;  // Fills the wielder with fervor to perform rapid and unrelenting strikes

        // Unique Slayer - Dancer's Crescent grants increased effectiveness against creatures of the night or shadows
        Slayer = SlayerName.Silver;  // Specifically effective against spectral and ethereal enemies, tying into the theme of fleeting beauty and shadowy enemies

        // Adding thematic visuals for an elegant weapon with a dangerous edge
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public DancersCrescent(Serial serial) : base(serial)
    {
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}
