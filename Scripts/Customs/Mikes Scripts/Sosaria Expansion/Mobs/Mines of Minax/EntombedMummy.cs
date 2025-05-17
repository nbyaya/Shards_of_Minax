using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an entombed mummy corpse")]
    public class EntombedMummy : BaseCreature
    {
        private DateTime m_NextSandstormTime;
        private DateTime m_NextCurseTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // A ghostly green tint
        private const int UniqueHue = 2101;

        [Constructable]
        public EntombedMummy() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.3, 0.6)
        {
            Name = "an entombed mummy";
            Body = 0x9B;
            BaseSoundID = 0x1D7;
            Hue = UniqueHue;

            // Stats
            SetStr(500, 550);
            SetDex(200, 250);
            SetInt(700, 800);

            SetHits(2000, 2300);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(18, 24);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold,     30);
            SetDamageType(ResistanceType.Energy,   20);
            SetDamageType(ResistanceType.Poison,   20);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     30, 40);
            SetResistance(ResistanceType.Cold,     60, 80);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   80, 90);

            SetSkill(SkillName.Wrestling,   100.0, 110.0);
            SetSkill(SkillName.Tactics,     95.0, 105.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Magery,      90.0, 100.0);
            SetSkill(SkillName.EvalInt,     95.0, 105.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Schedule first casts
            m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextCurseTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSummonTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));

            m_LastLocation = this.Location;

            // Themed loot
            PackItem(new GraveDust(Utility.RandomMinMax(5, 10)));
            PackItem(new BonePile(1));
        }

        // ❖ Chilling Aura & Poisonous Footfalls ❖
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && this.Alive && m.InRange(this.Location, 1) && m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
            {
                // 20% chance to chill stamina
                if (Utility.RandomDouble() < 0.2)
                {
                    DoHarmful(target);
                    target.SendMessage(0x48, "The mummy's tomb aura saps your strength!");
                    target.Stam -= Utility.RandomMinMax(10, 20);
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }

                // 10% chance to leave a poisonous tile
                if (Utility.RandomDouble() < 0.1)
                {
                    var p = new PoisonTile();
                    p.Hue = UniqueHue;
                    p.MoveToWorld(target.Location, this.Map);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile target && this.Map != null && this.Alive)
            {
                var now = DateTime.UtcNow;

                if (now >= m_NextSandstormTime && this.InRange(target.Location, 8))
                {
                    SandstormAttack();
                    m_NextSandstormTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                }
                else if (now >= m_NextCurseTime && this.InRange(target.Location, 12))
                {
                    CurseWaveAttack();
                    m_NextCurseTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
                }
                else if (now >= m_NextSummonTime)
                {
                    ScarabSummon();
                    m_NextSummonTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 60));
                }
            }

            // Leave quicksand behind as it moves
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                var oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                var qs = new QuicksandTile();
                qs.Hue = UniqueHue;
                qs.MoveToWorld(oldLoc, this.Map);
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        // — Sandstorm: AoE physical + poison + slow  
        private void SandstormAttack()
        {
            this.Say("*Rise, sands of torment!*");
            PlaySound(0x1FE);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 20, 60, UniqueHue, 0, 5052, 0
            );

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(this.Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 50), 40, 0, 0, 20, 40);

                if (m is Mobile t)
                {
                    t.SendMessage("Your feet sink and slow in the whipping sands!");
                    t.BodyMod = 0x3C;   // temporary slow visual
                    Timer.DelayCall(TimeSpan.FromSeconds(5), () => t.BodyMod = -1);
                }
            }
        }

        // — Curse Wave: Single‐target deadly poison + visual  
        private void CurseWaveAttack()
        {
            if (!(Combatant is Mobile target)) return;

            this.Say("*Feel the curse of the embalmed!*");
            PlaySound(0x22F);
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration),
                0x3728, 10, 20, UniqueHue, 0, 5039, 0
            );

            if (target is Mobile t)
            {
                DoHarmful(t);
                t.ApplyPoison(this, Poison.Deadly);
                t.SendMessage(0x22, "A deadly curse courses through your veins!");
            }
        }

        // — Scarab Summon: Calls 3–5 scarabs to fight for it  
        private void ScarabSummon()
        {
            this.Say("*Arise, my guardians!*");
            PlaySound(0x23C);

            for (int i = 0; i < Utility.RandomMinMax(3, 5); i++)
            {
                var scarab = new Skeleton();
                var loc = new Point3D(
                    X + Utility.RandomMinMax(-2, 2),
                    Y + Utility.RandomMinMax(-2, 2),
                    Z
                );

                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    scarab.MoveToWorld(loc, this.Map);
            }
        }

        // — Death Effect: Necro‑flames erupt all around  
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*My tomb claims you!*");
                PlaySound(0x1FE);
                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 15, 50, UniqueHue, 0, 5052, 0
                );

                for (int i = 0; i < Utility.RandomMinMax(3, 6); i++)
                {
                    var loc = new Point3D(
                        X + Utility.RandomMinMax(-3, 3),
                        Y + Utility.RandomMinMax(-3, 3),
                        Z
                    );

                    if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                        loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                    if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    {
                        var nf = new NecromanticFlamestrikeTile();
                        nf.Hue = UniqueHue;
                        nf.MoveToWorld(loc, this.Map);
                    }
                }
            }

            base.OnDeath(c);
        }

        // Standard overrides
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus      => 75.0;

        // Loot
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));
            AddLoot(LootPack.HighScrolls);
            if (Utility.RandomDouble() < 0.05)
                PackItem(new PowerCrystal());
        }

        public EntombedMummy(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns if the server reloads
            m_NextSandstormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextCurseTime     = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSummonTime    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            m_LastLocation      = this.Location;
        }
    }
}
