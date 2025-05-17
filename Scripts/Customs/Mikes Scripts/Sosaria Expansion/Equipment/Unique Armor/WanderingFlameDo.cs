using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WanderingFlameDo : LeatherDo
{
    [Constructable]
    public WanderingFlameDo()
    {
        Name = "Do of the Wandering Flame";
        Hue = Utility.Random(1500, 2300); // A warm, fiery color for the "wandering flame" theme
        BaseArmorRating = Utility.RandomMinMax(25, 50); // Medium armor rating, fitting for a leather piece

        // Attributes that fit the wandering and flame theme
        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        Attributes.BonusHits = 15;

        // Regeneration bonuses for stamina, enhancing mobility and endurance
        Attributes.RegenStam = 3;

        // Boost to evade or avoid damage in dynamic combat
        Attributes.DefendChance = 10;

        // Increased fire resistance, tying to the "wandering flame" concept
        FireBonus = 20;

        // Skill bonuses based on survival and movement
        SkillBonuses.SetValues(0, SkillName.Stealth, 15.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);
        SkillBonuses.SetValues(2, SkillName.Tracking, 10.0);

        // Xml level item to indicate its uniqueness
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WanderingFlameDo(Serial serial) : base(serial)
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
