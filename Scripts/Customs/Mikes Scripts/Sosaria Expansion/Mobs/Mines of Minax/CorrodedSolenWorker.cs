using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a corroded solen worker corpse")]
    public class CorrodedSolenWorker : BaseCreature
    {
        // Ability cooldowns
        private DateTime m_NextAcidSpray;
        private DateTime m_NextMiteSummon;
        private DateTime m_NextCorrosiveAura;

        // Corroded‑green hue
        private const int CorrodedHue = 0x8A0;

        [Constructable]
        public CorrodedSolenWorker()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a corroded solen worker";
            Body = 805;
            BaseSoundID = 959;
            Hue = CorrodedHue;

            // --- Enhanced Stats ---
            SetStr(300, 350);
            SetDex(100, 150);
            SetInt(150, 200);

            SetHits(900, 1100);
            SetStam(200, 250);
            SetMana(200, 300);

            SetDamage(12, 18);

            // Damage split
            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Poisoning, 120.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 80.0, 90.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 60;
            ControlSlots = 3;

            // Initialize cooldowns
            m_NextAcidSpray   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMiteSummon  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextCorrosiveAura = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));

            // Basic loot
            PackItem(new PoisonPotion());
            PackItem(new PoisonPotion());
            PackGold(Utility.RandomMinMax(200, 400));
        }

        // --- Corrosive Aura: poison burn when someone moves too close ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (DateTime.UtcNow >= m_NextCorrosiveAura
                && m != null && m != this 
                && m.Map == this.Map 
                && m.InRange(this.Location, 2) 
                && m.Alive && this.Alive 
                && CanBeHarmful(m, false))
            {
                if (m is Mobile targetMobile)
                {
                    DoHarmful(targetMobile);
                    int damage = Utility.RandomMinMax(10, 20);
                    // 100% poison damage
                    AOS.Damage(targetMobile, this, damage, 0, 0, 0, damage, 0);
                    targetMobile.SendMessage("The corroded solen's aura burns your flesh!");
                    targetMobile.PlaySound(0x1CF);
                }

                m_NextCorrosiveAura = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            }
        }

        // --- Main AI loop: check for Acidic Spray & Mite Summon ---
        public override void OnThink()
        {
            base.OnThink();

            if (Map == null || Map == Map.Internal || !Alive)
                return;

            // Acidic Spray (ranged AoE)
            if (DateTime.UtcNow >= m_NextAcidSpray
                && Combatant is Mobile target 
                && InRange(target.Location, 8))
            {
                AcidicSpray(target);
                m_NextAcidSpray = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }

            // Summon Corrosion Mites (adds)
            if (DateTime.UtcNow >= m_NextMiteSummon)
            {
                SummonCorrosionMites();
                m_NextMiteSummon = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Acidic Spray ability ---
        private void AcidicSpray(Mobile target)
        {
            Say("Sssss... acid strike!");
            PlaySound(0x33D);

            // Visual projectile
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, Location, Map),
                new Entity(Serial.Zero, target.Location, Map),
                0x36D4, 7, 0, false, false, CorrodedHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

            // AoE impact
            List<Mobile> list = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(target.Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && m is Mobile tgt 
                    && CanBeHarmful(tgt, false) 
                    && tgt.InRange(target.Location, 3))
                {
                    list.Add(tgt);
                }
            }
            eable.Free();

            foreach (Mobile m in list)
            {
                DoHarmful(m);
                int dmg = Utility.RandomMinMax(20, 35);
                // 100% poison
                AOS.Damage(m, this, dmg, 0, 0, 0, dmg, 0);
                m.FixedParticles(0x374A, 10, 15, 5032, CorrodedHue, 0, EffectLayer.Head);
                m.PlaySound(0x229);
            }
        }

        // --- Summon Corrosion Mites ---
        private void SummonCorrosionMites()
        {
            Say("Come forth, my brood!");
            PlaySound(0x2E6);

            for (int i = 0; i < 3; i++)
            {
                Point3D loc = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                // Assumes CorrosionMite is defined elsewhere
                AcidElemental mite = new AcidElemental();
                mite.Hue = CorrodedHue;
                mite.MoveToWorld(loc, Map);
            }
        }

        // --- On-death: spawn lingering poison pools ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map != null)
            {
                int poolCount = Utility.RandomMinMax(2, 4);
                for (int i = 0; i < poolCount; i++)
                {
                    Point3D loc = new Point3D(
                        X + Utility.RandomMinMax(-1, 1),
                        Y + Utility.RandomMinMax(-1, 1),
                        Z);

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    PoisonTile tile = new PoisonTile();
                    tile.Hue = CorrodedHue;
                    tile.MoveToWorld(loc, Map);
                }
            }
        }

        // --- Loot ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 3));

            if (Utility.RandomDouble() < 0.05)
                PackItem(new GreaterPoisonPotion());
        }

        public CorrodedSolenWorker(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on reload
            m_NextAcidSpray   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMiteSummon  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextCorrosiveAura = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
        }
    }
}
