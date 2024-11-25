using System;
using Server;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;


namespace Server.Mobiles
{
    [CorpseName("corpse of a test creature")]
    public class TestCreature2 : BaseCreature
    {
        [Constructable]
        public TestCreature2() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0x190; // Set a standard creature body
            Hue = 0; // Default hue

            Name = "Test Creature 2";
			
			Team = 2;

            SetStr(100);
            SetDex(100);
            SetInt(100);
            SetHits(9999);

            BaseSoundID = 0x5A; // Set a standard sound ID

            SetResistance(ResistanceType.Physical, 30);
            SetResistance(ResistanceType.Fire, 30);
            SetResistance(ResistanceType.Cold, 30);
            SetResistance(ResistanceType.Poison, 30);
            SetResistance(ResistanceType.Energy, 30);

            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 50.0);
            SetSkill(SkillName.Magery, 50.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 50.0);
            SetSkill(SkillName.Wrestling, 50.0);

            Tamable = false;
            ControlSlots = 1;
            MinTameSkill = 0;
			
			CanMove = true;

            // Attach the XmlAxeCircle attachment
			XmlAttach.AttachTo(this, new XmlRandomAbility());



            PackGold(100, 200); // Pack some gold
        }	


        public TestCreature2(Serial serial) : base(serial)
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
}
