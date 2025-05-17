using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Skullveil : WarAxe
{
    [Constructable]
    public Skullveil()
    {
        Name = "Skullveil";
        Hue = Utility.Random(1150, 1180);  // A sinister dark hue, evoking death and shadow
        MinDamage = Utility.RandomMinMax(35, 55);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.WeaponSpeed = 5;
        Attributes.Luck = 10;

        // Slayer effect - Skullveil is particularly effective against undead, emphasizing its dark and cursed nature
        Slayer = SlayerName.Exorcism;

        // Weapon attributes enhancing its combat effectiveness
        WeaponAttributes.HitLeechHits = 20; // Leech hits to drain life from enemies
        WeaponAttributes.HitLeechStam = 15; // Leech stamina to exhaust foes
        WeaponAttributes.HitLowerDefend = 10; // Decrease the opponent’s defense

        // Skill bonuses related to intimidation and combat
        SkillBonuses.SetValues(0, SkillName.Tactics, 15.0); // Improve tactical advantage in battle
        SkillBonuses.SetValues(1, SkillName.Lumberjacking, 20.0); // Enhance the use of blunt weapons, fitting for a brutal war axe
        SkillBonuses.SetValues(2, SkillName.Anatomy, 10.0); // Aid in targeting weak spots for more lethal blows

        // Thematic combat ability: Skullveil’s true power comes from dealing with the undead and spirits
        WeaponAttributes.HitDispel = 30; // Dispel summoned spirits or dark magic
        WeaponAttributes.HitHarm = 15; // Deal harm to enemies, making it effective in battle against cursed foes

        // Attach the XmlLevelItem for any potential XML-level integrations
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Skullveil(Serial serial) : base(serial)
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
