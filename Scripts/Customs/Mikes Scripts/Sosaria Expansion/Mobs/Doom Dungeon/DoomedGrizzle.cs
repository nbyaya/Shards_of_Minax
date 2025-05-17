using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a doomed grizzle corpse")]
    public class DoomedGrizzle : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextAcidRupture;
        private DateTime m_NextDoomEcho;
        private DateTime m_NextGrasp;
        private Point3D m_LastLocation;

        // Unique sickly‑green hue
        private const int UniqueHue = 1176;

        [Constructable]
        public DoomedGrizzle()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a doomed grizzle";
            Body = 259;
            Hue = UniqueHue;

            // Base stats
            SetStr(600, 650);
            SetDex(250, 300);
            SetInt(900, 1000);

            SetHits(2000, 2300);
            SetStam(200);
            SetMana(800, 900);

            SetDamage(20, 24);

            // Damage types
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Poison, 10);
            SetDamageType(ResistanceType.Energy, 20);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 60);
            SetResistance(ResistanceType.Cold, 60, 85);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 70, 90);

            // Skills
            SetSkill(SkillName.EvalInt, 120.0, 130.0);
            SetSkill(SkillName.Magery, 120.0, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 135.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 35000;
            Karma = -35000;

            VirtualArmor = 100;
            ControlSlots = 6;

            // Initialize cooldowns
            m_NextAcidRupture = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextDoomEcho    = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextGrasp       = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_LastLocation    = this.Location;

            // Loot: reagents + chance at Grizzled Ring of Doom
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            if (Utility.RandomDouble() < 0.02)
                PackItem(new GrizzledRingOfDread());
        }

        // ——— Hazardous Trail ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            // Leave a toxic gas cloud under anyone moving near it
            if (m is Mobile target && Alive && target.InRange(this, 2) && CanBeHarmful(target, false))
            {
                DoHarmful(target);

                // Drop a short‑lived ToxicGasTile at the grizzle’s feet
                if (Utility.RandomDouble() < 0.15)
                {
                    ToxicGasTile gas = new ToxicGasTile();
                    gas.Hue = UniqueHue;
                    gas.MoveToWorld(this.Location, this.Map);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // ——— Main AI Loop ———
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Acidic Rupture: 8‑tile cone, AoE poison + tile
            if (DateTime.UtcNow >= m_NextAcidRupture && InRange(Combatant.Location, 8))
            {
                AcidRupture();
                m_NextAcidRupture = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Doom Echo: radial fear + damage
            else if (DateTime.UtcNow >= m_NextDoomEcho && InRange(Combatant.Location, 12))
            {
                DoomEcho();
                m_NextDoomEcho = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            }
            // Tentacle Grasp: single‑target snare + stamina drain
            else if (DateTime.UtcNow >= m_NextGrasp && InRange(Combatant.Location, 6))
            {
                TentacleGrasp();
                m_NextGrasp = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }

            // Cursed Ground: leave a trap every time it moves
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.25)
            {
                TrapWeb web = new TrapWeb();
                web.Hue = UniqueHue;
                web.MoveToWorld(m_LastLocation, this.Map);
            }
            m_LastLocation = this.Location;
        }

        // ——— Acidic Rupture ———
        private void AcidRupture()
        {
            if (Map == null) return;
            Say("*The ground boils with corrosive ichor!*");
            PlaySound(0x22F);

            // AoE cone effect
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 8, 30, UniqueHue, 0, 5032, 0);

            foreach (Mobile m in Map.GetMobilesInRange(Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 100, 0);

                    // Spawn PoisonTile under each
                    PoisonTile pt = new PoisonTile();
                    pt.Hue = UniqueHue;
                    pt.MoveToWorld(m.Location, Map);
                }
            }
        }

        // ——— Doom Echo ———
        private void DoomEcho()
        {
            Say("*Your souls will wail!*");
            PlaySound(0x214);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && m is Mobile target && CanBeHarmful(target, false))
                    targets.Add(target);
            }

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                AOS.Damage(target, this, Utility.RandomMinMax(20, 35), 100, 0, 0, 0, 0);
                target.Freeze(TimeSpan.FromSeconds(2));      // brief chill
                target.SendMessage(0x22, "A dreadful resonance chills your bones!");
            }
        }

        // ——— Tentacle Grasp ———
        private void TentacleGrasp()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            Say("*Feel my grip!*");
            PlaySound(0x1F6);

            // Visual tentacle effect
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, Location, Map),
                new Entity(Serial.Zero, target.Location, Map),
                0x36BD, 5, 0, false, false, UniqueHue, 0, 9502, 0, 0, EffectLayer.Waist, 0x100);

            DoHarmful(target);
            target.Paralyze(TimeSpan.FromSeconds(3));
            target.Stam = Math.Max(0, target.Stam - Utility.RandomMinMax(30, 50));
            target.SendMessage(0x22, "Tendrils bind you and sap your strength!");
        }

        // ——— Death Explosion & Hazards ———
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            Say("*The doom within me is unleashed!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 12, 40, UniqueHue, 0, 5052, 0);

            // Spawn random hazards around corpse
            for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                Point3D loc = new Point3D(x, y, z);

                if (!Map.CanFit(x, y, z, 16, false, false))
                    loc.Z = Map.GetAverageZ(x, y);

                // Randomly pick between Landmine or Flamestrike
                if (Utility.RandomBool())
                {
                    LandmineTile mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(loc, Map);
                }
                else
                {
                    FlamestrikeHazardTile flame = new FlamestrikeHazardTile();
                    flame.Hue = UniqueHue;
                    flame.MoveToWorld(loc, Map);
                }
            }
        }

        // ——— Sounds ———
        public override int GetAngerSound()  => 0x581;
        public override int GetIdleSound()   => 0x582;
        public override int GetAttackSound() => 0x580;
        public override int GetHurtSound()   => 0x583;
        public override int GetDeathSound()  => 0x584;

        // ——— Loot & Properties ———
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 160.0;
        public override double DispelFocus     => 80.0;
        public override bool BleedImmune       => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,        Utility.RandomMinMax(8, 12));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
        }

        // ——— Serialization ———
        public DoomedGrizzle(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on reload
            m_NextAcidRupture = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextDoomEcho    = DateTime.UtcNow + TimeSpan.FromSeconds(15);
            m_NextGrasp       = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_LastLocation    = this.Location;
        }
    }
}
