using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a cave avenger corpse")]
    public class CaveAvenger : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextQuakeTime;
        private DateTime m_NextSporeBurstTime;
        private DateTime m_NextStoneRiftTime;
        private Point3D m_LastLocation;

        // A glowing amber‑green hue for subterranean menace
        private const int UniqueHue = 1365;

        [Constructable]
        public CaveAvenger() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name           = "a cave avenger";
            Body           = 152;
            BaseSoundID    = 0x24D;
            Hue            = UniqueHue;
            
            // Stats
            SetStr(550, 650);
            SetDex(120, 150);
            SetInt(300, 350);

            SetHits(2000, 2300);
            SetStam(200, 250);
            SetMana(400, 500);

            SetDamage(25, 35);

            // Damage types: physical + poison + energy
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Poison,   30);
            SetDamageType(ResistanceType.Energy,   30);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     45, 55);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   60, 70);

            // Skills
            SetSkill(SkillName.Magery,     100.0, 120.0);
            SetSkill(SkillName.EvalInt,    100.0, 120.0);
            SetSkill(SkillName.MagicResist,110.0, 130.0);
            SetSkill(SkillName.Tactics,     95.0, 105.0);
            SetSkill(SkillName.Wrestling,   95.0, 105.0);
            SetSkill(SkillName.Poisoning,   80.0, 100.0);

            Fame            = 25000;
            Karma           = -25000;
            VirtualArmor    = 90;
            ControlSlots    = 6;

            // Initialize cooldowns
            var now = DateTime.UtcNow;
            m_NextQuakeTime     = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSporeBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextStoneRiftTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation      = this.Location;

            // Starter loot
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 25)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 25)));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Aura of spore toxicity: any creature who moves within 2 tiles gets poison
            if (m != this && Alive && m.InRange(Location, 2) && CanBeHarmful(m, false))
            {
                if (SpellHelper.ValidIndirectTarget(this, m) && m is Mobile target)
                {
                    // Spray of toxic spores
                    target.FixedParticles(0x373A, 10, 15, 5012, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x2F5);
                    target.SendMessage("Toxic spores cloud your lungs!");

                    // Apply moderate poison
                    target.ApplyPoison(this, Poison.Lethal);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            var now = DateTime.UtcNow;

            // Stone Quake: close‐range tremor
            if (now >= m_NextQuakeTime && this.InRange(Combatant.Location, 6))
            {
                StoneQuake();
                m_NextQuakeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Spore Burst: medium‐range poison cloud
            else if (now >= m_NextSporeBurstTime && this.InRange(Combatant.Location, 12))
            {
                SporeBurst();
                m_NextSporeBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Stone Rift: targeted ground hazard
            else if (now >= m_NextStoneRiftTime)
            {
                StoneRift();
                m_NextStoneRiftTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Ability #1: Stone Quake ---
        private void StoneQuake()
        {
            this.Say("*The cavern trembles!*");
            this.PlaySound(0x1F1);

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3BB0, 20, 10, UniqueHue, 0, 5023, 0);

            var list = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    list.Add(m);
            }

            foreach (Mobile m in list)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(40, 60), 100, 0, 0, 0, 0);
                if (m is Mobile target)
                    target.Freeze(TimeSpan.FromSeconds(Utility.RandomMinMax(2, 4)));
            }
        }

        // --- Ability #2: Spore Burst ---
        private void SporeBurst()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*Breathe deep...*");
                this.PlaySound(0x2F6);

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, this.Location, Map),
                    new Entity(Serial.Zero, target.Location, Map),
                    0x3696, 10, 0, false, false, UniqueHue, 0, 9508, 1, 0, EffectLayer.Waist, 0x100);

                DoHarmful(target);
                int damage = Utility.RandomMinMax(30, 50);
                AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);
                target.ApplyPoison(this, Poison.Deadly);
            }
        }

        // --- Ability #3: Stone Rift Hazard ---
        private void StoneRift()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            var loc = target.Location;
            this.Say("*The earth tears!*");
            this.PlaySound(0x2A8);

            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 8, 8, UniqueHue, 0, 5034, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null) return;

                // Drop a StoneSpikeTile (custom damaging tile)
                HotLavaTile spike = new HotLavaTile();
                spike.Hue = UniqueHue;
                spike.MoveToWorld(loc, Map);

                Effects.PlaySound(loc, Map, 0x214);
            });
        }

        // --- Death Explosion: Final Reckoning ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            this.Say("*My vengeance endures…*");
            Effects.PlaySound(Location, Map, 0x212);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 12, 40, UniqueHue, 0, 5052, 0);

            // Scatter toxic ground patches
            for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
            {
                int dx = Utility.RandomMinMax(-3, 3), dy = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + dx, Y + dy, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                PoisonTile gas = new PoisonTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, Map);
            }
        }

        // --- Loot & Rewards ---
        public override int TreasureMapLevel { get { return 6; } }
        public override bool BleedImmune    { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int Meat            { get { return 4; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,         Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls,  Utility.RandomMinMax(1, 2));

            // 3% chance for a unique artifact
            if (Utility.RandomDouble() < 0.03)
                PackItem(new SilentVowOfTheWatcher());
        }

        // --- Serialization Boilerplate ---
        public CaveAvenger(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re‑init timers after load
            var now = DateTime.UtcNow;
            m_NextQuakeTime      = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSporeBurstTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextStoneRiftTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
