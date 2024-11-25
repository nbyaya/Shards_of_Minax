using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the cowboy overlord")]
    public class CowboyBoss : Cowboy
    {
        [Constructable]
        public CowboyBoss() : base()
        {
            Name = "Cowboy Overlord";
            Title = "the Supreme Outlaw";

            // Update stats to match or exceed Barracoon's levels
            SetStr(1200); // Match or exceed Barracoon's upper strength
            SetDex(255); // Max dexterity as provided in the base cowboy stats
            SetInt(250); // Max intelligence as provided in the base cowboy stats

            SetHits(12000); // Much higher than the original
            SetDamage(29, 38); // Match Barracoon's damage range

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100); // No change, already high
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 110.0);
            SetSkill(SkillName.EvalInt, 110.0);
            SetSkill(SkillName.Meditation, 100.0);

            Fame = 25000; // Increase fame to make it a more renowned boss
            Karma = -25000; // Negative karma, fitting for a boss
            VirtualArmor = 75; // Increased virtual armor to make it more resilient

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
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

        public override void OnThink()
        {
            base.OnThink();
            // Additional logic could be added here, e.g., if you want special boss behavior
        }

        public CowboyBoss(Serial serial) : base(serial)
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
