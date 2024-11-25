using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss azure moose corpse")]
    public class BossAzureMoose : AzureMoose
    {
        [Constructable]
        public BossAzureMoose()
        {
            Name = "Azure Moose";
            Title = "the Colossus";
            Hue = 1991; // Azure hue, may be customized
            Body = 0xEA; // GreatHart body

            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(16000); // High hit points to match a boss
            SetDamage(35, 50); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90); // Improved resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Negative karma for a boss

            VirtualArmor = 100; // Improved armor

            Tamable = false; // Boss cannot be tamed
            ControlSlots = 0; // No control slots needed

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Drop 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a valid item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("Feel the wrath of the Azure Moose!");
            PackGold(2000, 3000); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities
        }

        public BossAzureMoose(Serial serial) : base(serial)
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
