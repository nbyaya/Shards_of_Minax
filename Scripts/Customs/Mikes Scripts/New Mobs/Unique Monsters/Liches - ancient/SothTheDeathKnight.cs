using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a death knight's corpse")]
    public class SothTheDeathKnight : BaseCreature
    {
        private DateTime m_NextUnholySmite;
        private DateTime m_NextCursedAura;
        private DateTime m_NextRitualOfDarkness;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public SothTheDeathKnight()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Soth, the Death Knight";
            Body = 78; // Ancient Lich body
            Hue = 2094; // Fiery red hue for the dark aura
			BaseSoundID = 412;

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
            PackNecroReg(100, 200);

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SothTheDeathKnight(Serial serial)
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
                    m_NextUnholySmite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextCursedAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextRitualOfDarkness = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextUnholySmite)
                {
                    UnholySmite();
                }

                if (DateTime.UtcNow >= m_NextCursedAura)
                {
                    CursedAura();
                }

                if (DateTime.UtcNow >= m_NextRitualOfDarkness)
                {
                    RitualOfDarkness();
                }

                if (Hits < HitsMax * 0.3) // Trigger if health is below 30%
                {
                    PerformRitual();
                }
            }

            ApplyDeathlyPresence();
        }

        private void UnholySmite()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Soth, the Death Knight smites you with unholy power! *");
            PlaySound(0x1F8); // Dark power sound

            if (Combatant != null && Combatant is Mobile mobileCombatant && mobileCombatant.Karma < 0) // Damage only if target has positive karma
            {
                int extraDamage = 60; // Adjust as needed
                AOS.Damage(mobileCombatant, this, extraDamage, 0, 100, 0, 0, 0);
                mobileCombatant.SendMessage("You are struck by a powerful unholy smite!");
                
                // Chance to summon minions
                if (Utility.RandomDouble() < 0.25) // 25% chance
                {
                    SummonMinions();
                }
            }

            m_NextUnholySmite = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for UnholySmite
        }

        private void SummonMinions()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Soth summons the spirits of fallen knights! *");
            PlaySound(0x1F4); // Summon sound

            for (int i = 0; i < 2; i++)
            {
                BaseCreature minion = new UndeadKnight(); // Assuming you have a class for UndeadKnight
                minion.MoveToWorld(Location, Map);
                minion.Combatant = Combatant;
            }
        }

        private void CursedAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A dark aura surrounds Soth, lowering your defenses! *");
            PlaySound(0x1F7); // Aura effect sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && CanBeHarmful(m))
                {
                    m.SendMessage("Your defenses are lowered by the cursed aura!");

                    // Temporarily reduce resistances using AddResistanceMod
                    AddResistanceMod(m, "CursedAura-Physical", ResistanceType.Physical, -20, TimeSpan.FromSeconds(15));
                    AddResistanceMod(m, "CursedAura-Fire", ResistanceType.Fire, -20, TimeSpan.FromSeconds(15));
                    AddResistanceMod(m, "CursedAura-Cold", ResistanceType.Cold, -20, TimeSpan.FromSeconds(15));
                    AddResistanceMod(m, "CursedAura-Poison", ResistanceType.Poison, -20, TimeSpan.FromSeconds(15));
                    AddResistanceMod(m, "CursedAura-Energy", ResistanceType.Energy, -20, TimeSpan.FromSeconds(15));
                }
            }

            m_NextCursedAura = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for CursedAura
        }

        private void AddResistanceMod(Mobile m, string name, ResistanceType resistanceType, int offset, TimeSpan duration)
        {
            ResistanceMod mod = new ResistanceMod(resistanceType, offset);
            m.AddResistanceMod(mod);

            Timer.DelayCall(duration, () =>
            {
                m.RemoveResistanceMod(mod);
                m.SendMessage($"{name} effect has worn off.");
            });
        }

        private void RitualOfDarkness()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Soth performs a dark ritual, summoning dark energies! *");
            PlaySound(0x1F6); // Ritual sound

            // Effects and damage
            Effects.SendLocationEffect(Location, Map, 0x3709, 20, 10); // Dark ritual visual effect

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive)
                {
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0); // Pure dark damage
                    m.SendMessage("You are struck by dark energies from the ritual!");
                }
            }

            m_NextRitualOfDarkness = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for RitualOfDarkness
        }

        private void PerformRitual()
        {
            if (Utility.RandomDouble() < 0.5) // 50% chance
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Soth begins a powerful ritual! *");
                PlaySound(0x1F5); // Ritual sound

                // Summon a powerful minion
                BaseCreature ritualMinion = new Balron(); // Replace with a suitable class if needed
                ritualMinion.MoveToWorld(Location, Map);
                ritualMinion.Combatant = Combatant;
            }
        }

        private void ApplyDeathlyPresence()
        {
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    if (m is PlayerMobile)
                    {
                        m.SendMessage("You feel a chill as Soth's deathly presence bears down on you.");
                        m.Freeze(TimeSpan.FromSeconds(2)); // Freeze effect for 2 seconds
                    }
                }
            }
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
