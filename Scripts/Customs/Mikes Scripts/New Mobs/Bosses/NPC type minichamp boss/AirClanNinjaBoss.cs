using System;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the air clan grandmaster")]
    public class AirClanNinjaBoss : AirClanNinja
    {
        [Constructable]
        public AirClanNinjaBoss() : base()
        {
            Name = "Air Clan Grandmaster";
            Title = "the Tempest Whisperer";

            // Update stats to match or exceed Barracoon's stats
            SetStr(650); // Stronger than before, surpassing Barracoon's strength
            SetDex(700); // Higher dexterity for speed and agility
            SetInt(800); // Maximum intelligence, enhancing magical and stealth abilities
            SetHits(12000); // Matching or exceeding Barracoon's health for a boss-tier fight

            SetDamage(120, 150); // Increased damage output for a more challenging fight

            SetResistance(ResistanceType.Physical, 75); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 80); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 80); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 75); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 80); // Increased energy resistance

            SetSkill(SkillName.EvalInt, 200.0); // Higher magic resistance and offense
            SetSkill(SkillName.Magery, 200.0);  // Enhanced magical abilities
            SetSkill(SkillName.MagicResist, 200.0); // Max magic resistance
            SetSkill(SkillName.Tactics, 150.0);  // Advanced tactical skills
            SetSkill(SkillName.Fencing, 150.0);  // High weapon skill
            SetSkill(SkillName.Ninjitsu, 200.0); // Higher Ninjitsu skill (if your shard supports it)
            SetSkill(SkillName.Stealth, 200.0);  // Top-tier stealth for sneaky combat

            Fame = 30000; // Higher fame value for a boss
            Karma = 5000; // More positive karma for this higher-tier ninja

            VirtualArmor = 75; // Higher armor value for better protection

            // Attach the random ability
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

            // You could add more themed loot specific to this ninja, such as air-themed items
            // Example: PackItem(new Feather(Utility.Random(1, 3))); // Add a few feathers to match the air theme
        }

        public override void OnThink()
        {
            base.OnThink();

            // You can add additional logic for the boss's behavior, e.g., special attacks or speech
        }

        public AirClanNinjaBoss(Serial serial) : base(serial)
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
