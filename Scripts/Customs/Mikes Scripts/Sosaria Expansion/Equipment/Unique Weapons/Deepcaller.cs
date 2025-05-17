using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Deepcaller : FishermansTrident
{
    [Constructable]
    public Deepcaller()
    {
        Name = "Deepcaller";
        Hue = Utility.Random(1200, 1300); // Dark aquatic hues, reminiscent of the deep sea
        MinDamage = Utility.RandomMinMax(30, 55);
        MaxDamage = Utility.RandomMinMax(60, 90);

        // Attributes tailored to its deep-sea, elemental theme
        Attributes.WeaponSpeed = 10;
        Attributes.Luck = 15;
        Attributes.DefendChance = 10;
        
        // Thematic Slayer - Deepcaller is especially potent against sea creatures, tied to water lore
        Slayer = SlayerName.DragonSlaying;  // Represents ancient marine creatures, like dragons of the sea
        
        // Weapon attributes specific to the sea and elemental powers
        WeaponAttributes.HitLeechMana = 20;
        WeaponAttributes.HitLeechHits = 15;
        WeaponAttributes.HitLeechStam = 10;
        WeaponAttributes.BattleLust = 15;

        // Skill bonuses for thematic synergy
        SkillBonuses.SetValues(0, SkillName.Fishing, 25.0); // Makes you a better fisherman when wielding this
        SkillBonuses.SetValues(1, SkillName.Tactics, 10.0); // Enhances combat ability with a trident
        SkillBonuses.SetValues(2, SkillName.Mysticism, 15.0); // Taps into the ancient mystic powers of the sea
        
        // Additional thematic bonus - the trident calls upon elemental water energies
        SkillBonuses.SetValues(3, SkillName.MagicResist, 10.0); // Protects you from magical water-based attacks

        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Deepcaller(Serial serial) : base(serial)
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
