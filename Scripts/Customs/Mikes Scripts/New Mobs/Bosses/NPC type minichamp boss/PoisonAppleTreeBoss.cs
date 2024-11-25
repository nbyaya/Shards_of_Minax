using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the poison apple overlord")]
    public class PoisonAppleTreeBoss : PoisonAppleTree
    {
        [Constructable]
        public PoisonAppleTreeBoss() : base()
        {
            Name = "Poison Apple Overlord";
            Title = "the Corruptor of the Orchard";
            Hue = 2174; // Poison green color, remains same as original
            Team = 1; // Boss team

            // Update stats to match or exceed those of a high-level boss
            SetStr(425); // Increased strength
            SetDex(150); // Increased dexterity
            SetInt(750); // Increased intelligence

            SetHits(12000); // Enhanced health
            SetDamage(29, 40); // Increased damage

            SetResistance(ResistanceType.Physical, 70, 80); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90); // Poison is the main element
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 120.0); // Increased Magic Resist skill
            SetSkill(SkillName.Tactics, 120.0); // Increased Tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Increased Wrestling skill

            Fame = 25000; // Increased Fame
            Karma = -25000; // Negative Karma for a villainous boss

            VirtualArmor = 80; // Increased armor value

            // Attach the XmlRandomAbility for dynamic abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Scheduled method to perform the special attack
            Timer.DelayCall(TimeSpan.FromSeconds(4.0), TimeSpan.FromSeconds(8.0), ThrowPoisonApple);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        // Override OnThink if you want additional boss behavior here
        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic (e.g., passive abilities, status effects, etc.)
        }

        public PoisonAppleTreeBoss(Serial serial) : base(serial)
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
