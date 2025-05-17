using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; 

namespace Server.Mobiles
{
    [CorpseName("a meatformer corpse")]
    public class Meatformer : BaseCreature
    {
        private DateTime m_NextHookTime;
        private DateTime m_NextBurstTime;
        private DateTime m_NextRendTime;
        private DateTime m_NextRegenTime;
        private Point3D m_LastLocation;

        // A visceral, sickly-red flesh hue
        private const int UniqueHue = 2968;

        [Constructable]
        public Meatformer() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "the Meatformer";
            Body = 306;
            BaseSoundID = 0x2A7;
            Hue = UniqueHue;

            // Massive physical powerhouse with some arcane resilience
            SetStr(600, 700);
            SetDex(200, 250);
            SetInt(350, 450);

            SetHits(2000, 2400);
            SetStam(300, 350);
            SetMana(200, 300);

            SetDamage(25, 35);

            // Damage profile: mostly physical, some poison
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Poison, 20);

            // Tough resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 40, 60);

            // Skills tuned for raw brutality
            SetSkill(SkillName.Wrestling, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 115.0);
            SetSkill(SkillName.Poisoning, 100.0, 115.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize cooldowns
            m_NextHookTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBurstTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextRendTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextRegenTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

            m_LastLocation = this.Location;

            // Standard loot packs plus chance for rare "Fleshcore"
            PackItem(new RawRibs(Utility.RandomMinMax(10, 20)));
            PackItem(new Bandage(Utility.RandomMinMax(5, 10)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(20, 30)));
        }

        public Meatformer(Serial serial) : base(serial)
        {
        }

        // --- Regenerative Aura: heals when enemies draw near ---
        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= m_NextRegenTime && Alive && Combatant != null && Map != null && Map != Map.Internal)
            {
                // Check for any Mobile target within 4 tiles
                IPooledEnumerable mobs = Map.GetMobilesInRange(Location, 4);
                foreach (Mobile m in mobs)
                {
                    if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    {
                        int heal = Utility.RandomMinMax(50, 80);
                        Hits = Math.Min(Hits + heal, HitsMax);
                        FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                        PlaySound(0x228);
                        break;
                    }
                }
                mobs.Free();
                m_NextRegenTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }

            // --- Ability cooldown checks ---
            if (Combatant != null && Alive && Map != null && Map != Map.Internal)
            {
                // Meat Hook: ranged pull + heavy damage
                if (DateTime.UtcNow >= m_NextHookTime && InRange(Combatant.Location, 12))
                {
                    MeatHookAttack();
                    m_NextHookTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                }
                // Blood Burst: AoE physical + poison explosion
                else if (DateTime.UtcNow >= m_NextBurstTime && InRange(Combatant.Location, 6))
                {
                    BloodBurstAttack();
                    m_NextBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
                }
                // Flesh Rend: scatter toxic meat pools around target
                else if (DateTime.UtcNow >= m_NextRendTime && InRange(Combatant.Location, 10))
                {
                    FleshRendAttack();
                    m_NextRendTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }
            }
        }

        // --- Meat Hook: pulls the target in, deals damage, applies slow and poison ---
        public void MeatHookAttack()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*The chains of flesh bind you!*");
                PlaySound(0x227);
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, Location, Map),
                    new Entity(Serial.Zero, target.Location, Map),
                    0x36D4, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

                DoHarmful(target);
                int damage = Utility.RandomMinMax(80, 120);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0); // 100% physical

                // Pull target next to Meatformer
                target.Location = Location;
                target.ProcessDelta();

                // Apply slow effect
                target.SendMessage(0x22, "Your flesh chains drag you down!");
                target.Poison = Poison.Lesser;
            }
        }

        // --- Blood Burst: cone/AoE explosion of gore ---
        public void BloodBurstAttack()
        {
            Say("*Feel the flood of my blood!*");
            PlaySound(0x229);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 15, 30, UniqueHue, 0, 5032, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile t in targets)
            {
                DoHarmful(t);
                int dmg = Utility.RandomMinMax(40, 60);
                AOS.Damage(t, this, dmg, 80, 0, 0, 20, 0); // 80% phys, 20% poison
                t.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                t.PlaySound(0x231);
            }
        }

        // --- Flesh Rend: scatters toxic meat pools around the combatant ---
        public void FleshRendAttack()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Sear yourself on my flesh!*");
                PlaySound(0x22B);

                // Drop several ToxicGasTiles around the target
                for (int i = 0; i < 5; i++)
                {
                    int dx = Utility.RandomMinMax(-2, 2);
                    int dy = Utility.RandomMinMax(-2, 2);
                    Point3D loc = new Point3D(target.X + dx, target.Y + dy, target.Z);
                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    ToxicGasTile gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(loc, Map);
                }
            }
        }

        // --- Leave a trail of Toxic Blood ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && Alive && m.Map == Map && m.InRange(Location, 2) && CanBeHarmful(m, false))
            {
                if (Utility.RandomDouble() < 0.2 && Map != null)
                {
                    Point3D drop = oldLocation;
                    if (!Map.CanFit(drop.X, drop.Y, drop.Z, 16, false, false))
                        drop.Z = Map.GetAverageZ(drop.X, drop.Y);

                    ToxicGasTile pool = new ToxicGasTile();
                    pool.Hue = UniqueHue;
                    pool.MoveToWorld(drop, Map);
                }
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            // Final gore eruption
            PlaySound(0x22D);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 20, 40, UniqueHue, 0, 5032, 0);

            // Spawn extra blood pools
            for (int i = 0; i < 6; i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                Point3D loc = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                ToxicGasTile gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, Map);
            }
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 6; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            // Small chance for a rare "Fleshcore" component
            if (Utility.RandomDouble() < 0.05)
                PackItem(new SulfurousAsh(1)); // placeholder for Fleshcore item
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

            // Reset timers on load
            m_NextHookTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextRendTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextRegenTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
        }
    }
}
