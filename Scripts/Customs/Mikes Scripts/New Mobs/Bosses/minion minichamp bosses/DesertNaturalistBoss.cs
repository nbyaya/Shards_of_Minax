using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the desert overlord")]
    public class DesertNaturalistBoss : DesertNaturalist
    {
        private TimeSpan m_SpellDelay = TimeSpan.FromSeconds(5.0); // Reduced spell delay for the boss

        [Constructable]
        public DesertNaturalistBoss() : base()
        {
            Name = "Desert Overlord";
            Title = "the Supreme Naturalist";

            // Update stats to match or exceed the original NPC's values
            SetStr(800, 1200); // Higher strength
            SetDex(200, 250); // Higher dexterity
            SetInt(300, 400); // Higher intelligence

            SetHits(8000, 12000); // Increased health
            SetDamage(20, 40); // Increased damage

            SetResistance(ResistanceType.Physical, 60, 80); // Increased resistance
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 90, 100); // Poison resistance maxed
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.EvalInt, 120.0, 140.0); // Higher magic skill
            SetSkill(SkillName.Magery, 120.0, 140.0); 
            SetSkill(SkillName.Meditation, 80.0, 100.0); 
            SetSkill(SkillName.MagicResist, 95.0, 115.0); // Higher magic resistance
            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 12000; // Higher fame
            Karma = -12000; // Negative karma for boss-tier status

            VirtualArmor = 60; // Increased virtual armor

            // Attach a random ability to enhance the fight experience
            XmlAttach.AttachTo(this, new XmlRandomAbility());


            // Additional boss loot
            PackItem(new Sandals(Utility.RandomNeutralHue()));
            PackItem(new Robe(Utility.RandomNeutralHue()));
            if (!this.Female)
            {
                PackItem(new ShortPants(Utility.RandomNeutralHue()));
            }
            else
            {
                PackItem(new Kilt(Utility.RandomNeutralHue()));
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new SkullCircleMateria());
			PackItem(new BeggingAugmentCrystal());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional logic for the boss to summon desert creatures (enhanced boss behavior)
            if (DateTime.Now >= m_NextSpellTime && Combatant != null)
            {
                Mobile target = this.Combatant as Mobile;

                if (target != null && target.Map == this.Map && target.InRange(this, 12))
                {
                    // Summon a desert creature
                    switch (Utility.Random(3))
                    {
                        case 0:
                            Summon(new Scorpion(), target);
                            break;
                        case 1:
                            Summon(new Snake(), target);
                            break;
                        case 2:
                            Summon(new EarthElemental(), target); // New summon type for variety
                            break;
                    }

                    m_NextSpellTime = DateTime.Now + m_SpellDelay;
                }
            }
        }

        private void Summon(BaseCreature creature, Mobile target)
        {
            Map map = this.Map;
            if (map != null)
            {
                Point3D location = new Point3D(target.X + 2, target.Y + 2, target.Z);
                creature.MoveToWorld(location, map);
                creature.Combatant = target;
                this.Say(true, "Arise, my creature, and serve me in battle!");
            }
        }

        public DesertNaturalistBoss(Serial serial) : base(serial)
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
