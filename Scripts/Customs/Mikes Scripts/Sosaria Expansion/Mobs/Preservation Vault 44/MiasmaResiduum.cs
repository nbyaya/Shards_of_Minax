using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a miasma residuum corpse")]
    public class MiasmaResiduum : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextNovaTime;
        private DateTime m_NextChainTime;
        private DateTime m_NextSwarmTime;
        private Point3D m_LastLocation;

        // Unique greenish‑black hue for the miasma
        private const int MiasmaHue = 1351;

        [Constructable]
        public MiasmaResiduum() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name = "a miasma residuum";
            Body = 273;
            BaseSoundID = 0x56b; // idle sound
            Hue = MiasmaHue;

            // --- Stats ---
            SetStr(350, 450);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(2000, 2500);
            SetStam(300, 350);
            SetMana(800, 1000);

            SetDamage(25, 35);

            // --- Damage Types ---
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Poison, 80);
            SetDamageType(ResistanceType.Energy, 20);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 80, 90);

            // --- Skills ---
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.EvalInt,   120.0, 130.0);
            SetSkill(SkillName.Magery,    120.0, 130.0);
            SetSkill(SkillName.Poisoning, 110.0, 120.0);
            SetSkill(SkillName.MagicResist,115.0,125.0);
            SetSkill(SkillName.Tactics,   100.0, 110.0);
            SetSkill(SkillName.Wrestling,  90.0, 100.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize cooldowns
            m_NextNovaTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSwarmTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_LastLocation  = this.Location;

            // Standard loot: reagents, gems, scrolls
            PackItem( new BlackPearl( Utility.RandomMinMax(15, 20 ) ) );
            PackItem( new Nightshade( Utility.RandomMinMax(15, 20 ) ) );
            PackItem( new SulfurousAsh( Utility.RandomMinMax(15, 20 ) ) );

        }

        // --- Aura: Leaves toxic gas behind as it moves ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != this && m.Map == this.Map && m.InRange(this.Location, 3) && m.Alive && CanBeHarmful(m, false))
            {
                // spawn a short‐lived ToxicGasTile at Residuum's feet
                if (Map.CanFit(this.X, this.Y, this.Z, 16, false, false))
                {
                    ToxicGasTile gas = new ToxicGasTile();
                    gas.Hue = MiasmaHue;
                    gas.MoveToWorld(this.Location, this.Map);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            DateTime now = DateTime.UtcNow;

            if (now >= m_NextNovaTime && this.InRange(Combatant.Location, 8))
            {
                CastNecroticNova();
                m_NextNovaTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextChainTime && this.InRange(Combatant.Location, 12))
            {
                CastResidualChain();
                m_NextChainTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
            else if (now >= m_NextSwarmTime)
            {
                CastMiasmaSwarm();
                m_NextSwarmTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
            }
        }

        // --- Ability: Necrotic Nova (AoE poison + energy burst) ---
        private void CastNecroticNova()
        {
            Say("*The air crackles with deathly vapors!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x375A, 12, 30, MiasmaHue, 0, 5032, 0);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (Mobile enemy in targets)
            {
                DoHarmful(enemy);

                // 50–80 poison, 30–50 energy damage
                int dmg = Utility.RandomMinMax(50, 80);
                AOS.Damage(enemy, this, dmg, 0, 0, 0, dmg, 0);

                // 30% chance to apply Deadly poison
                if (Utility.RandomDouble() < 0.30 && enemy is Mobile target)
                {
                    target.ApplyPoison(this, Poison.Deadly);
                }
            }
        }

        // --- Ability: Residual Chain (poison bolt bounces between targets) ---
        private void CastResidualChain()
        {
            if (!(Combatant is Mobile initialTarget) || !CanBeHarmful(initialTarget, false))
                return;

            Say("*Corrosive tendrils latch onto you!*");
            PlaySound(0x20A);

            List<Mobile> chain = new List<Mobile> { initialTarget };
            int maxChains = 4;

            for (int i = 0; i < maxChains; i++)
            {
                Mobile last = chain[chain.Count - 1];
                Mobile next = null;
                double closest = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, 6))
                {
                    if (m != this && m != last && !chain.Contains(m)
                        && CanBeHarmful(m, false)
                        && SpellHelper.ValidIndirectTarget(this, m)
                        && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < closest)
                        {
                            closest = d;
                            next = m;
                        }
                    }
                }

                if (next != null)
                    chain.Add(next);
                else
                    break;
            }

            for (int i = 0; i < chain.Count; i++)
            {
                Mobile from = (i == 0 ? this : chain[i - 1]);
                Mobile to   = chain[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, from.Location, from.Map),
                    new Entity(Serial.Zero, to.Location,   to.Map),
                    0x36D4, 7, 0, false, false, MiasmaHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.1 * i), () =>
                {
                    if (CanBeHarmful(to, false))
                    {
                        DoHarmful(to);
                        int boltDmg = Utility.RandomMinMax(40, 60);
                        AOS.Damage(to, this, boltDmg, 0, 0, 0, boltDmg, 0);

                        // leave a poison tile under target
                        if (Map.CanFit(to.X, to.Y, to.Z, 16, false, false))
                        {
                            PoisonTile pt = new PoisonTile();
                            pt.Hue = MiasmaHue;
                            pt.MoveToWorld(to.Location, to.Map);
                        }
                    }
                });
            }
        }

        // --- Ability: Miasma Swarm (spawns toxic gas hazards around area) ---
        private void CastMiasmaSwarm()
        {
            Say("*Rise, my children of plague!*");
            PlaySound(0x22F);

            for (int i = 0; i < 5; i++)
            {
                int x = this.X + Utility.RandomMinMax(-5, 5);
                int y = this.Y + Utility.RandomMinMax(-5, 5);
                int z = this.Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                ToxicGasTile swarm = new ToxicGasTile();
                swarm.Hue = MiasmaHue;
                swarm.MoveToWorld(new Point3D(x, y, z), this.Map);
                Timer.DelayCall(TimeSpan.FromSeconds(Utility.RandomDouble() * 1.5), () =>
                {
                    PlaySound(0x5CE); // subtle hiss
                });
            }
        }

        // --- Death Effect: Miasmic Collapse ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;

            Say("*My essence... dissipates...*");
            PlaySound(0x56e);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 12, 40, MiasmaHue, 0, 5052, 0);

            // Spawn several poison and toxic gas zones
            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                PoisonTile pt = new PoisonTile();
                pt.Hue = MiasmaHue;
                pt.MoveToWorld(new Point3D(x, y, z), Map);

                ToxicGasTile gt = new ToxicGasTile();
                gt.Hue = MiasmaHue;
                gt.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus     { get { return 75.0;  } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,       Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls,Utility.RandomMinMax(1, 3));

            if (Utility.RandomDouble() < 0.05) // 5% chance
                PackItem(new StormstepGuards());   // Unique artifact
        }

        public MiasmaResiduum(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // reinit cooldowns
            m_NextNovaTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextChainTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextSwarmTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
        }

        public override int GetAngerSound()  { return 0x56d; }
        public override int GetIdleSound()   { return 0x56b; }
        public override int GetAttackSound() { return 0x56c; }
        public override int GetHurtSound()   { return 0x56c; }
        public override int GetDeathSound()  { return 0x56e; }
    }
}
