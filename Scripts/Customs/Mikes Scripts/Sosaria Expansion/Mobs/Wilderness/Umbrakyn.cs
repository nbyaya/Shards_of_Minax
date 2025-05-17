using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("an umbrakyn corpse")]
    public class Umbrakyn : BaseCreature
    {
        private DateTime _NextShadowPulse;
        private DateTime _NextLifeDrain;
        private DateTime _NextShadowStep;
        private DateTime _NextShadowAuraTick;

        [Constructable]
        public Umbrakyn()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Umbrakyn";
            Body = 0x58E;
            BaseSoundID = 362;
            Female = true;
            Hue = 1195;

            SetStr(1000, 1200);
            SetDex(150, 200);
            SetInt(700, 850);

            SetHits(1200, 1500);
            SetDamage(18, 25);

            // No Chaos in ServUO by default
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 25);
            SetDamageType(ResistanceType.Poison, 25);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     70, 80);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   75, 85);

            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Tactics,     110.0, 120.0);
            SetSkill(SkillName.Wrestling,   110.0, 120.0);
            SetSkill(SkillName.DetectHidden,100.0);
            SetSkill(SkillName.Magery,      115.0, 125.0);
            SetSkill(SkillName.EvalInt,     115.0, 125.0);
            SetSkill(SkillName.Meditation,  100.0, 110.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 60;

            Tamable = false;
            ControlSlots = 0;

            _NextShadowPulse    = DateTime.UtcNow;
            _NextLifeDrain      = DateTime.UtcNow;
            _NextShadowStep     = DateTime.UtcNow;
            _NextShadowAuraTick = DateTime.UtcNow;
        }

        public Umbrakyn(Serial serial) : base(serial) { }

        public override bool AutoDispel => true;
        public override bool ReacquireOnMovement => true;
        public override int TreasureMapLevel => 6;

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (!(Combatant is Mobile target) || target.Deleted || target.Map != Map || !InRange(target, 18) || !CanBeHarmful(target))
                return;

            // Shadow Pulse (AOE)
            if (DateTime.UtcNow >= _NextShadowPulse && InRange(target, 10) && InLOS(target))
            {
                bool foundGroup = false;
                foreach (var m in target.GetMobilesInRange(6))
                {
                    if (m != target && m != this && CanBeHarmful(m))
                    {
                        foundGroup = true;
                        break;
                    }
                }

                if (foundGroup || Utility.RandomDouble() < 0.3)
                {
                    UseShadowPulse(target);
                    _NextShadowPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                    return;
                }
            }

            // Life Drain (single)
            if (DateTime.UtcNow >= _NextLifeDrain
             && InRange(target, 8) && InLOS(target)
             && Hits < HitsMax * 0.75)
            {
                UseLifeDrain(target);
                _NextLifeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                return;
            }
            else if (DateTime.UtcNow >= _NextLifeDrain && Utility.RandomDouble() < 0.1)
            {
                UseLifeDrain(target);
                _NextLifeDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
                return;
            }

            // Shadow Step (teleport)
            if (DateTime.UtcNow >= _NextShadowStep && Utility.RandomDouble() < 0.15)
            {
                UseShadowStep(target);
                _NextShadowStep = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 35));
                return;
            }

            // Shadow Aura (periodic)
            if (DateTime.UtcNow >= _NextShadowAuraTick)
            {
                ApplyShadowAura();
                _NextShadowAuraTick = DateTime.UtcNow + TimeSpan.FromSeconds(5.0);
            }
        }

        private void UseShadowPulse(Mobile target)
        {
            if (Deleted || !Alive || target.Deleted || !InRange(target, 10) || !InLOS(target) || !CanBeHarmful(target))
                return;

            // Wind‑up
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                0x3709,  // graphic
                10,      // speed
                30,      // duration
                0        // effect (none)
            ); // :contentReference[oaicite:0]{index=0}
            PlaySound(0x65A);

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                if (Deleted || !Alive || target.Deleted) return;

                // Explosion
                Effects.SendLocationParticles(
                    EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                    0x3728,
                    10,
                    10,
                    0
                ); // :contentReference[oaicite:1]{index=1}
                Effects.PlaySound(target.Location, target.Map, 0x217);

                int damage = Utility.RandomMinMax(30, 45);
                var list = new List<Mobile>();

                foreach (var m in target.GetMobilesInRange(8))
                {
                    if (m != this && CanBeHarmful(m))
                    {
                        DoHarmful(m);
                        list.Add(m);
                    }
                }

                foreach (var m in list)
                {
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100);
                    var mod = new ResistanceMod(ResistanceType.Physical, -15);
                    m.AddResistanceMod(mod);
                    Timer.DelayCall(TimeSpan.FromSeconds(8.0), () => m.RemoveResistanceMod(mod));
                }
            });
        }

        private void UseLifeDrain(Mobile target)
        {
            if (Deleted || !Alive || target.Deleted || !InRange(target, 8) || !InLOS(target) || !CanBeHarmful(target))
                return;

            DoHarmful(target);

            // Drain effect
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                0x37B9,
                10,
                0,
                0
            ); // :contentReference[oaicite:2]{index=2}
            PlaySound(0x231);

            int damage = Utility.RandomMinMax(20, 35);
            AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

            Heal((int)(damage * 0.75));
        }

        private void UseShadowStep(Mobile target)
        {
            if (Deleted || !Alive || target.Deleted || !InRange(target, 18))
                return;

            // Disappear
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728,
                10,
                10,
                0
            ); // :contentReference[oaicite:3]{index=3}
            Effects.PlaySound(Location, Map, 0x217);

            // Teleport
            Point3D dest = Location;
            for (int i = 0; i < 10; i++)
            {
                var cand = target.Location;
                cand.X += Utility.RandomMinMax(-4, 4);
                cand.Y += Utility.RandomMinMax(-4, 4);

                if (Map.CanSpawnMobile(cand))
                {
                    dest = cand;
                    break;
                }
            }

            MoveToWorld(dest, Map);

            // Reappear
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728,
                10,
                10,
                0
            ); // :contentReference[oaicite:4]{index=4}
            Effects.PlaySound(Location, Map, 0x217);
        }

        private void ApplyShadowAura()
        {
            if (Deleted || !Alive) return;

            foreach (var m in GetMobilesInRange(8))
            {
                if (m != this && CanBeHarmful(m) && (m.Player || m.Body.IsAnimal || m.Body.IsMonster))
                {
                    DoHarmful(m);

                    var swing = new StatMod(StatType.Dex, "UmbrakynSwingDebuff", -(m.Dex / 10), TimeSpan.FromSeconds(6));
                    var cast  = new StatMod(StatType.Int, "UmbrakynCastDebuff", -(m.Int / 10), TimeSpan.FromSeconds(6));
                    bool found = false;

                    foreach (var mod in m.StatMods)
                        if (mod.Name == swing.Name || mod.Name == cast.Name)
                        { found = true; break; }

                    if (!found)
                    {
                        m.AddStatMod(swing);
                        m.AddStatMod(cast);
                        m.SendMessage("You feel the oppressive shadow of the Umbrakyn!");
                    }

                    // Aura particles
                    Effects.SendTargetParticles(
                        m,
                        0x3709,       // graphic
                        10,           // speed
                        30,           // duration
                        0,            // effect
                        EffectLayer.Waist // layer
                    ); // :contentReference[oaicite:5]{index=5}
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.HighScrolls, 3);
            AddLoot(LootPack.Gems, 2);

            if (Utility.RandomDouble() < 0.10)
                PackItem(new GoldRing());

            if (Utility.RandomDouble() < 0.05)
                PackItem(new SulfurousAsh(10));
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
