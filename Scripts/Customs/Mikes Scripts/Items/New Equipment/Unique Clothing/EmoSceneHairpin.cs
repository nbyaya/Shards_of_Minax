using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class EmoSceneHairpin : BodySash
{
    [Constructable]
    public EmoSceneHairpin()
    {
        Name = "Emo Scene Hairpin";
        Hue = Utility.Random(700, 2700);
        Attributes.ReflectPhysical = 5;
        ClothingAttributes.MageArmor = 1;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
        Resistances.Chaos = 10;
        Resistances.Physical = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public EmoSceneHairpin(Serial serial) : base(serial)
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
