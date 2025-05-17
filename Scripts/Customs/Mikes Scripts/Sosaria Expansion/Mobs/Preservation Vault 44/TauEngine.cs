using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a shattered Tau-Engine corpse")]
    public class TauEngine : BaseCreature
    {
        private DateTime m_NextCorrodeTime;
        private DateTime m_NextSurgeTime;
        private DateTime m_NextRamTime;
        private DateTime m_NextNexusTime;
        private Point3D m_LastLocation;

        // A cold‑steel cyan hue for its mechanical shell
        private const int UniqueHue = 1175;

        [Constructable]
        public TauEngine() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Tau-Engine";
            Body = Utility.RandomList(0xE8, 0xE9);  // Bull bodies
            BaseSoundID = 0x64;                    // Bull sounds
            Hue = UniqueHue;

            // —— Core Stats ——
            SetStr(550, 700);
            SetDex(200, 250);
            SetInt(250, 350);

            SetHits(1200, 1500);
            SetStam(250, 300);
            SetMana(500, 650);

            SetDamage(20, 30);

            // —— Damage Distribution ——
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 10);
            SetDamageType(ResistanceType.Energy, 50);

            // —— Resistances ——
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            // —— Skills ——
            SetSkill(SkillName.MagicResist, 110.0, 120.0);
            SetSkill(SkillName.Tactics,      100.0, 110.0);
            SetSkill(SkillName.Wrestling,    100.0, 110.0);
            SetSkill(SkillName.EvalInt,       90.0, 100.0);
            SetSkill(SkillName.Magery,        90.0, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize cooldowns
            m_NextCorrodeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_NextSurgeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextRamTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextNexusTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

            m_LastLocation = this.Location;

            // Base loot
            PackItem(new MaxxiaDust(Utility.RandomMinMax(5, 10)));
            PackItem(new MaxxiaDust(Utility.RandomMinMax(3, 6)));
        }

        // —— Corrosive Aura —— drains stamina + poisons on anyone moving too close
        public override void OnMovement(Mobile m, Point3D oldLoc)
        {
            if (m != this && m.Map == this.Map && Alive && m.InRange(Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    // Drain stamina
                    int drain = Utility.RandomMinMax(15, 25);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage(0x22, "The Tau-Engine’s gears grind your resolve!");
                        target.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                        target.PlaySound(0x1F8);
                    }

                    // Apply lingering poison
                    target.ApplyPoison(this, Poison.Deadly);
                }
            }

            base.OnMovement(m, oldLoc);
        }

        public override void OnThink()
        {
            base.OnThink();

            Mobile target = Combatant as Mobile;
			if (target == null || Map == null || Map == Map.Internal || !Alive)
				return;


            // Leave behind arc‑steel shrapnel tiles as it walks
            if (Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var old = m_LastLocation;
                m_LastLocation = Location;

                Point3D spawn = old;
                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                var shard = new HotLavaTile(); // using lava tile as molten metal
                shard.Hue = UniqueHue;
                shard.MoveToWorld(spawn, Map);
            }
            else
            {
                m_LastLocation = Location;
            }

            // —— Ability scheduling ——
            DateTime now = DateTime.UtcNow;

            if (now >= m_NextRamTime && InRange(target.Location, 4))
            {
                MechanicalRam(target);
                m_NextRamTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextCorrodeTime && InRange(target.Location, 6))
            {
                ArcaneOverloadSurge();
                m_NextCorrodeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
            else if (now >= m_NextSurgeTime && InRange(target.Location, 12))
            {
                SummonEnergyStorm();
                m_NextSurgeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (now >= m_NextNexusTime && InRange(target.Location, 14))
            {
                VortexNexus(target);
                m_NextNexusTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        // —— Crush with blinding ram ——
        public void MechanicalRam(Mobile target)
        {
            if (!CanBeHarmful(target, false)) return;

            Say("*🔊 CLANK-CRUSH!*");
            PlaySound(0x213);

            // Knockback
            int damage = Utility.RandomMinMax(50, 80);
            target.Move(GetDirectionTo(target.Location));
            AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
            target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Waist);
        }

        // —— Burst of arcane‑metallic energy (AoE) ——
        public void ArcaneOverloadSurge()
        {
            Say("*⚡ Overload surge!*");
            PlaySound(0x213);

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 12, 60, UniqueHue, 0, 5039, 0);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 0, 0, 0, 0, 100);

                if (Utility.RandomDouble() < 0.5 && m is Mobile tm)
                {
                    tm.Mana = Math.Max(0, tm.Mana - Utility.RandomMinMax(20, 35));
                    tm.SendMessage(0x22, "Your mind is battered by crackling energy!");
                    tm.FixedParticles(0x374A, 10, 15, 5032, EffectLayer.Head);
                    tm.PlaySound(0x1F8);
                }
            }
        }

        // —— Summon random energy‑storm tiles around itself ——
        public void SummonEnergyStorm()
        {
            Say("*Engaging storm protocol!*");
            PlaySound(0x20A);

            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                int x = X + Utility.RandomMinMax(-6, 6);
                int y = Y + Utility.RandomMinMax(-6, 6);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var storm = new LightningStormTile();
                storm.Hue = UniqueHue;
                storm.MoveToWorld(new Point3D(x, y, z), Map);

                Effects.PlaySound(new Point3D(x, y, z), Map, 0x299);
            }
        }

        // —— Tear open a vortex at the target’s feet ——
        public void VortexNexus(Mobile target)
        {
            if (!CanBeHarmful(target, false)) return;

            Say("*Reality fracture!*");
            PlaySound(0x22F);

            Point3D loc = target.Location;
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            // Delay before vortex appears
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                var tile = new VortexTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, Map);
                Effects.PlaySound(loc, Map, 0x1F6);
            });
        }

        // —— Death explosion spawns necro‑tiles ——
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;
			
			Say("*System failure…*");
            Effects.PlaySound(Location, Map, 0x213);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
            {
                int x = X + Utility.RandomMinMax(-4, 4);
                int y = Y + Utility.RandomMinMax(-4, 4);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var flame = new NecromanticFlamestrikeTile();
                flame.Hue = UniqueHue;
                flame.MoveToWorld(new Point3D(x, y, z), Map);
            }

            
        }

        // —— Loot & Serialization ——
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));
            if (Utility.RandomDouble() < 0.03)
                PackItem(new VoidCore());
        }

        public override bool BleedImmune    => true;
        public override int  TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 75.0;

        public TauEngine(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑initialize timers
            m_NextCorrodeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_NextSurgeTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            m_NextRamTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextNexusTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
