using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the inferno master")]
    public class FireClanNinjaBoss : FireClanNinja
    {
        [Constructable]
        public FireClanNinjaBoss() : base()
        {
            Name = "Inferno Master";
            Title = "the Supreme Shadow of Flames";

            // Update stats to match or exceed the original FireClanNinja, but with boss-tier numbers
            SetStr(950); // Increasing Strength
            SetDex(650); // Increasing Dexterity
            SetInt(400); // Increasing Intelligence
            SetHits(1500); // Matching the higher health of the FireClanNinja

            SetDamage(150, 160); // Increasing the damage range

            SetSkill(SkillName.MagicResist, 220.0); // Higher Magic Resist
            SetSkill(SkillName.Fencing, 200.0); // Higher skill in combat
            SetSkill(SkillName.Macing, 200.0); 
            SetSkill(SkillName.Swords, 200.0);
            SetSkill(SkillName.Tactics, 200.0);
            SetSkill(SkillName.Wrestling, 200.0);
            SetSkill(SkillName.Ninjitsu, 200.0); // If the shard has Ninjitsu skill, it's enhanced

            Fame = 20000; // Increased fame for a boss-tier enemy
            Karma = -10000; // Negative karma for a stronger enemy

            VirtualArmor = 90; // Boost virtual armor to make it tougher

            // Attach the XmlRandomAbility to give the boss some extra random abilities
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
            // Additional boss logic can be implemented here, like using special abilities or emotes
        }

        public FireClanNinjaBoss(Serial serial) : base(serial)
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
