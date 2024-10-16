using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Engines;

namespace Server.Mobiles
{
    [CorpseName("Tutankhamun's corpse")]
    public class TutankhamunTheCursed : BaseCreature
    {
        private DateTime m_NextCurse;
        private DateTime m_NextSummon;
        private DateTime m_NextDesertStorm;
        private DateTime m_NextVengeance;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public TutankhamunTheCursed()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Tutankhamun the Cursed";
            Body = 154; // Mummy body
            Hue = 2158; // Unique hue
			BaseSoundID = 471;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public TutankhamunTheCursed(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextCurse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSummon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDesertStorm = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextVengeance = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextCurse)
                {
                    PharaohsCurse();
                }

                if (DateTime.UtcNow >= m_NextSummon)
                {
                    MummifiedWrath();
                }

                if (DateTime.UtcNow >= m_NextDesertStorm)
                {
                    DesertStorm();
                }

                if (DateTime.UtcNow >= m_NextVengeance)
                {
                    MummysVengeance();
                }
            }
        }

        private void PharaohsCurse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Tutankhamun's gaze curses you with a Pharaoh's wrath! *");
            PlaySound(0x20E); // Cursed sound

            if (Combatant is Mobile target)
            {
                // Apply curse effect using poison
                ApplyPoison(target, Poison.Lesser);
                target.SendMessage("You feel weakened and slowed by the Pharaoh's curse!");

                // Reduce target's skills and stats
                target.Dex -= 15;
                target.Str -= 15;
                target.Int -= 15;
                target.SendMessage("You are cursed and your abilities are diminished!");

                // Apply visual effect
                Effects.SendTargetEffect(target, 0x1F4, 30); // Dark energy effect

                Timer.DelayCall(TimeSpan.FromSeconds(30), () =>
                {
                    if (target != null && !target.Deleted)
                    {
                        target.Dex += 15;
                        target.Str += 15;
                        target.Int += 15;
                    }
                });
            }

            m_NextCurse = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for PharaohsCurse
        }

        private void MummifiedWrath()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Tutankhamun summons a wave of undead minions! *");
            PlaySound(0x2A); // Summon sound

            // Summon minions (Skeletons or Zombies)
            for (int i = 0; i < 4; i++)
            {
                Mobile minion = Utility.RandomBool() ? (Mobile)new Skeleton() : new Zombie();
                minion.Hue = 0x455; // Dark particles or shadowy figure
                minion.MoveToWorld(Location, Map);
                minion.Combatant = Combatant;
            }

            // Add a shadowy area effect
            Effects.SendLocationEffect(Location, Map, 0x1F4, 30, 10);
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The undead minions disperse into shadows! *");

            m_NextSummon = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for MummifiedWrath
        }

        private void DesertStorm()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A fierce desert storm engulfs the area! *");
            PlaySound(0x1F4); // Storm sound

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 100, 0, 0); // Area damage
                    m.SendMessage("The desert storm blinds and burns you!");
                }
            }

            Effects.SendLocationEffect(Location, Map, 0x1F4, 30, 10); // Storm visual effect

            m_NextDesertStorm = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for DesertStorm
        }

        private void MummysVengeance()
        {
            if (Combatant is Mobile target)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Tutankhamun retaliates with a powerful strike! *");
                PlaySound(0x20F); // Retaliation sound

                // Powerful attack
                AOS.Damage(target, this, Utility.RandomMinMax(25, 40), 0, 0, 100, 0, 0);

                target.SendMessage("Tutankhamun's vengeance strikes you with great force!");
            }

            m_NextVengeance = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Cooldown for MummysVengeance
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
            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }
}
