using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a sentinel stinger corpse")]
    public class StingerClassSentinel : BaseCreature, IRepairableMobile
    {
        public Type RepairResource { get { return typeof(Server.Items.IronIngot); } }

        // Timers for special abilities
        private DateTime m_NextAcidSprayTime;
        private DateTime m_NextEMPTime;
        private DateTime m_NextOverchargeTime;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1350; // Vibrant metallic cyan

        [Constructable]
        public StingerClassSentinel()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Stinger-Class Sentinel";
            Body = 717;               // Clockwork scorpion chassis
            BaseSoundID = 541;        // Anger sound; idle/death/attack override below
            Hue = UniqueHue;

            // ——— Attributes ———
            SetStr(600, 700);         // Titan‐grade strength
            SetDex(200, 250);         // Fairly nimble
            SetInt(350, 400);         // Advanced targeting systems

            SetHits(2000, 2300);
            SetStam(300, 350);
            SetMana(0);               // Purely mechanical

            SetDamage(20, 30);

            // ——— Damage Types ———
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Poison, 30);

            // ——— Resistances ———
            SetResistance(ResistanceType.Physical, 85, 95);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   100);
            SetResistance(ResistanceType.Energy,   40, 50);

            // ——— Skills ———
            SetSkill(SkillName.Tactics,      120.1, 130.0);
            SetSkill(SkillName.Wrestling,     90.1, 100.0);
            SetSkill(SkillName.MagicResist,   80.1,  95.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 90;
            ControlSlots = 6;

            // Initialize cooldowns
            m_NextAcidSprayTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            m_NextEMPTime         = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextOverchargeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));

            m_LastLocation = this.Location;

            // Loot: high‐end reagents and rare Mechanist parts
            PackItem(new IronIngot(Utility.RandomMinMax(15, 25)));
            PackItem(new VoidCore(Utility.RandomMinMax(5, 10)));
            PackItem(new LightOfTheLastStar());
        }

        // ——— Passive Mine Drop ———
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Map == null || this.Map == Map.Internal || !Alive)
                return;

            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.15)
            {
                var loc = m_LastLocation;
                int z = loc.Z;

                if (!Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                    z = Map.GetAverageZ(loc.X, loc.Y);

                if (Map.CanFit(loc.X, loc.Y, z, 16, false, false))
                {
                    var mine = new LandmineTile();
                    mine.Hue = UniqueHue;
                    mine.MoveToWorld(new Point3D(loc.X, loc.Y, z), Map);
                }
            }

            m_LastLocation = this.Location;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            var now = DateTime.UtcNow;
			var target = Combatant as Mobile;
			double? rangeTo = target?.GetDistanceToSqrt(this);


            if (now >= m_NextOverchargeTime && rangeTo <= 4)
            {
                OverchargeDash();
                m_NextOverchargeTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            else if (now >= m_NextEMPTime && rangeTo <= 6)
            {
                EMPSurge();
                m_NextEMPTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextAcidSprayTime && rangeTo <= 8)
            {
                AcidSpray();
                m_NextAcidSprayTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
        }

        // ——— Special Attack: Acid Spray (ranged poison attack) ———
        public void AcidSpray()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*<hissing mechanical whirr>*");
            PlaySound(562);
            this.Direction = GetDirectionTo(target);

            // Particle projectile
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x36D4,  // Poison bolt
                7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
            {
                if (target.Alive && CanBeHarmful(target, false))
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 40, 60);
                    target.ApplyPoison(this, Poison.Lethal);
                    target.FixedParticles(0x375A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            });
        }

        // ——— Special Ability: EMP Surge (area mana/stamina drain + tile) ———
        public void EMPSurge()
        {
            Say("*<high‑frequency pulse>*");
            PlaySound(0x1F6);

            // Central effect
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                         0x376A, 12, 40, UniqueHue, 0, 5039, 0);

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(this.Location, 6))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);

                // Drain mana & stamina
                int drain = Utility.RandomMinMax(20, 35);
                if (m.Mana > 0)
                {
                    int manaLost = Math.Min(m.Mana, drain);
                    m.Mana -= manaLost;
                    m.SendMessage(0x22, "Your arcane circuits sputter and lose energy!");
                }

                if (m.Stam > 0)
                {
                    int stamLost = Math.Min(m.Stam, drain);
                    m.Stam -= stamLost;
                }

                m.FixedParticles(0x374A, 8, 12, 5032, UniqueHue, 0, EffectLayer.Head);

                // Place an EMP tile under them
                var tile = new LightningStormTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(m.Location, m.Map);
            }
        }

        // ——— Special Movement: Overcharge Dash (charge + stun) ———
        public void OverchargeDash()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*<revving servos>*");
            PlaySound(541);
            var oldDir = this.Direction;
            this.Direction = GetDirectionTo(target);
            this.Move(GetDirectionTo(target)); // step forward

            Timer.DelayCall(TimeSpan.FromSeconds(0.2), () =>
            {
                if (target.Alive && target.Map == this.Map && target.InRange(this, 1))
                {
                    DoHarmful(target);
                    target.Stam = 0;
                    target.Paralyze(TimeSpan.FromSeconds(3));
                    target.SendMessage(0x482, "You are stunned by the sentinel's overcharged impact!");
                    target.FixedParticles(0x3728, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                }
                this.Direction = oldDir;
            });
        }

        // ——— Sounds ———
        public override int GetAngerSound()   { return 541; }
        public override int GetIdleSound()    { return 542; }
        public override int GetDeathSound()   { return 545; }
        public override int GetAttackSound()  { return 562; }
        public override int GetHurtSound()    { return Controlled ? 320 : base.GetHurtSound(); }

        public override bool AutoDispel      { get { return !Controlled; } }
        public override bool BleedImmune     { get { return true; } }
        public override Poison PoisonImmune  { get { return Poison.Lethal; } }

        // ——— Loot ———
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,         Utility.RandomMinMax(10, 15));
            if (Utility.RandomDouble() < 0.01) // 1% for rare Mechanist component
                PackItem(new EmberhideWrap());
        }

        // ——— Serialization ———
        public StingerClassSentinel(Serial serial) : base(serial) { }

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
            m_NextAcidSprayTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            m_NextEMPTime         = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 24));
            m_NextOverchargeTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            m_LastLocation        = this.Location;
        }
    }
}
