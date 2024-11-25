using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the shadowy overlord")]
    public class SneakyNinjaBoss : SneakyNinja
    {
        [Constructable]
        public SneakyNinjaBoss() : base()
        {
            Name = "Shadowy Overlord";
            Title = "the Master Assassin";

            // Enhanced Stats
            SetStr(425); // Enhanced strength to match a boss-tier NPC
            SetDex(250); // Increased dexterity for better speed
            SetInt(750); // Higher intelligence

            SetHits(12000); // Matching Barracoon-like health
            SetDamage(29, 38); // Increased damage for a more challenging fight

            // Resistance values similar to Barracoon for increased durability
            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 70);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 75);
            SetResistance(ResistanceType.Energy, 70);

            // Increased skills to make the NPC a formidable opponent
            SetSkill(SkillName.Hiding, 100.0);
            SetSkill(SkillName.Stealth, 100.0);
            SetSkill(SkillName.Fencing, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Ninjitsu, 100.0);

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // Good armor for a boss-tier NPC

            // Attach a random ability for more unpredictability
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

            // Boss behavior enhancement (higher detection range and aggression)
            if (!Hidden && this.Combatant == null)
            {
                IPooledEnumerable eable = this.GetMobilesInRange(15); // Larger detection range
                foreach (Mobile m in eable)
                {
                    if (m.Player && m.InLOS(this) && m.AccessLevel == AccessLevel.Player)
                    {
                        this.Hidden = true; // Hide when a player is detected, for a surprise attack
                        break;
                    }
                }
                eable.Free();
            }
            else if (Hidden && this.Combatant != null)
            {
                this.RevealingAction(); // Reveal and initiate an attack when a player is in combat
            }
        }

        public SneakyNinjaBoss(Serial serial) : base(serial)
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
