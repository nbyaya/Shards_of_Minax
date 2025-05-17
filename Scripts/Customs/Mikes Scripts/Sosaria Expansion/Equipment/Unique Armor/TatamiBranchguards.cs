using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TatamiBranchguards : LeatherHiroSode
{
    [Constructable]
    public TatamiBranchguards()
    {
        Name = "Tatami Branchguards";
        Hue = Utility.Random(1200, 1300); // A natural earthy hue fitting the "Tatami" name
        BaseArmorRating = Utility.RandomMinMax(30, 55); // Mid-range defense to balance aesthetics and protection

        Attributes.BonusDex = 15; // Encourages agility and quick movement
        Attributes.BonusInt = 10; // A slight boost to intelligence, reflecting the item’s connection to nature and wisdom
        Attributes.RegenStam = 5; // Helps with stamina regeneration, ideal for quick actions

        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Emphasizing tactical knowledge in battle
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0); // Enhances the character’s ability to dodge and block
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // The Tatami Branchguards are finely crafted to allow for swift, calculated movements

        ColdBonus = 5; // Provides some resistance to cold, appropriate for the armor's natural materials
        FireBonus = 5; // Provides minor protection against fire, offering resilience in many environments

        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attaching the XmlLevelItem to ensure the item works with the leveling system
    }

    public TatamiBranchguards(Serial serial) : base(serial)
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
