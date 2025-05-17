using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a void priest corpse")]
    public class VoidPriest : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextNullAura;
        private DateTime m_NextVoidRift;
        private DateTime m_NextOblivionWave;
        private DateTime m_NextSummonShadows;

        // Track last location for movement effects
        private Point3D m_LastLocation;

        // A deep, unsettling void‑purple
        private const int UniqueHue = 1175;

        [Constructable]
        public VoidPriest() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a void priest";
            Body = 253;               // same as YomotsuPriest
            BaseSoundID = 0x452;      // same as YomotsuPriest
            Hue = UniqueHue;          // unique tint

            // — Stats —
            SetStr(600, 650);
            SetDex(150, 200);
            SetInt(700, 800);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(800, 900);

            SetDamage(12, 16);

            // — Damage Types —
            SetDamageType(ResistanceType.Physical, 15);
            SetDamageType(ResistanceType.Energy, 85);

            // — Resistances —
            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 90, 95);

            // — Skills —
            SetSkill(SkillName.EvalInt, 115.0, 130.0);
            SetSkill(SkillName.Magery, 115.0, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 135.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 95.0, 105.0);
            SetSkill(SkillName.Wrestling, 95.0, 105.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize ability cooldowns
            m_NextNullAura    = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextVoidRift    = DateTime.UtcNow + TimeSpan.FromSeconds(12);
            m_NextOblivionWave = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextSummonShadows = DateTime.UtcNow + TimeSpan.FromSeconds(30);

            m_LastLocation = this.Location;

            // — Loot —
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
        }

        // — Aura: Null Zone — drains stamina from anyone who comes too close
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (Alive && m != this && Map == m.Map && m.InRange(Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    int drain = Utility.RandomMinMax(10, 20);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage(0x22, "You feel your life force wrenched away by the void!"); 
                        target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Ensure we only target Mobiles when needed
            if (DateTime.UtcNow >= m_NextVoidRift && this.InRange(Combatant.Location, 12))
            {
                VoidRiftAttack();
                m_NextVoidRift = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (DateTime.UtcNow >= m_NextOblivionWave && this.InRange(Combatant.Location, 8))
            {
                OblivionWaveAttack();
                m_NextOblivionWave = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (DateTime.UtcNow >= m_NextSummonShadows)
            {
                SummonShadows();
                m_NextSummonShadows = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(45, 60));
            }

            // Movement‑based terrain effect (drop a VortexTile behind)
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var old = m_LastLocation;
                m_LastLocation = Location;

                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    var vortex = new VortexTile();
                    vortex.Hue = UniqueHue;
                    vortex.MoveToWorld(old, Map);
                }
            }
            else
            {
                m_LastLocation = Location;
            }
        }

        // — Ability #1: Void Rift — spawns a damaging vortex under the target
        public void VoidRiftAttack()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                Say("*Embrace the void!*");
                PlaySound(0x22F);

                var loc = target.Location;
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (Map == null) return;

                    var rift = new VortexTile();
                    rift.Hue = UniqueHue;
                    rift.MoveToWorld(loc, Map);

                    Effects.SendLocationParticles(
                        EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                        0x3728, 10, 10, UniqueHue, 0, 5039, 0);
                });
            }
        }

        // — Ability #2: Oblivion Wave — AoE dark burst that wounds and fears
        public void OblivionWaveAttack()
        {
            Say("*All shall be undone!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            var hits = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    hits.Add(m);
            }

            foreach (var m in hits)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 100);
                m.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);

                // 30% chance to quake their resolve (fear/paralyze)
                if (Utility.RandomDouble() < 0.30 && m is Mobile target)
                    target.Paralyze(TimeSpan.FromSeconds(3.0));
            }
        }

        // — Ability #3: Summon Shadows — call forth 1–3 wraiths to fight by its side
        public void SummonShadows()
        {
            Say("*Rise from darkness!*");
            PlaySound(0x1E7);

            int count = Utility.RandomMinMax(1, 3);
            for (int i = 0; i < count; i++)
            {
                var w = new Wraith();  // assume you have a ShadowWraith mob
                w.Hue = UniqueHue;
                w.MoveToWorld(
                    new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z),
                    Map);
                w.PlaySound(0x482);
            }
        }

        // — Death: Unraveling Voidburst —
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			
			if (Map == null) return;
			
			Say("*The void... claims all...*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            // scatter poisonous void miasma tiles
            for (int i = 0; i < Utility.RandomMinMax(4, 8); i++)
            {
                int dx = Utility.RandomMinMax(-4, 4), dy = Utility.RandomMinMax(-4, 4);
                var loc = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, Map);
            }

            base.OnDeath(c);
        }

        // — Standard Overrides & Loot —
        public override bool BleedImmune      { get { return true; } }
        public override int  TreasureMapLevel { get { return 6;    } }
        public override double DispelDifficulty{ get { return 150.0;} }
        public override double DispelFocus     { get { return 75.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            if (Utility.RandomDouble() < 0.05) // 5% chance for a Void Core
                PackItem(new VoidCore());
        }

        public override int GetIdleSound()  { return 0x42A; }
        public override int GetAttackSound(){ return 0x435; }
        public override int GetHurtSound()  { return 0x436; }
        public override int GetDeathSound() { return 0x43A; }

        public VoidPriest(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on server restart
            m_NextNullAura     = DateTime.UtcNow + TimeSpan.FromSeconds(5);
            m_NextVoidRift     = DateTime.UtcNow + TimeSpan.FromSeconds(12);
            m_NextOblivionWave = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextSummonShadows= DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }
    }
}
