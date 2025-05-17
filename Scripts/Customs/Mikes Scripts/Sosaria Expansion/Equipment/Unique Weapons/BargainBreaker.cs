using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BargainBreaker : MerchantsShotgun
{
    [Constructable]
    public BargainBreaker()
    {
        Name = "Bargain Breaker";
        Hue = Utility.Random(2200, 2300);  // A golden hue with hints of tarnished silver to give it a sleek yet old-world feel
        MinDamage = Utility.RandomMinMax(20, 40);
        MaxDamage = Utility.RandomMinMax(45, 70);
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;
        Attributes.WeaponDamage = 30;

        // Skill bonuses to emphasize the merchant aspect and tactical advantage in combat
        SkillBonuses.SetValues(0, SkillName.ItemID, 20.0);  // Bonus to Archery
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);   // Bonus to Tactics (strategic advantage in combat)
        SkillBonuses.SetValues(2, SkillName.Tracking, 15.0);  // Bonus to Tracking, as the merchant could be skilled in following deals and rivals

        // Slayer effect - Effective against certain creatures related to commerce or trade: Bandits
        Slayer = SlayerName.Vacuum;  // This could symbolize a weapon that's destructive and draws in chaos (like a vacuum) from ill-intentioned individuals.

        // Weapon attributes - adding "explosive" flair to the weapon
        WeaponAttributes.HitEnergyArea = 40;  // Shotgun blasts deal area damage
        WeaponAttributes.HitLeechHits = 25;   // Leech hits, perfect for the unpredictable trade of the underworld
        WeaponAttributes.HitLowerDefend = 20; // Lowers defenses, like pushing people out of business

        // Custom attributes for the item
        Attributes.LowerRegCost = 10;   // Bargain breaks give you "cheaper" access to magical energy
        Attributes.CastSpeed = 1;        // Faster use of magical abilities, representing how quickly a merchant can 'close a deal'

        // Additional thematic bonus: Commerce or Negotiation-based abilities
        SkillBonuses.SetValues(3, SkillName.Snooping, 20.0);  // Bonus to Snooping, perfect for a merchant with secrets to learn

        // Attaching the custom XML item level
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BargainBreaker(Serial serial) : base(serial)
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
