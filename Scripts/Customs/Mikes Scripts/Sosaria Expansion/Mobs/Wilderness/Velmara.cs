using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a Velmara corpse")]
    public class Velmara : BaseCreature
    {
        private DateTime _nextTempest;
        private DateTime _nextManaDrain;
        private DateTime _nextTeleport;
        private DateTime _nextSummon;

        [Constructable]
        public Velmara()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name           = "Velmara";
            Body           = 0x310;
            BaseSoundID    = 0x47D;
            Hue            = 0x48E; // Violet‑tinged aura

            // Stats
            SetStr(220, 250);
            SetDex(150, 180);
            SetInt(400, 450);

            SetHits(1200, 1400);
            SetMana(600, 700);

            // Basic melee
            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire,     25);
            SetDamageType(ResistanceType.Energy,   25);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     80, 90);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   60, 70);
            SetResistance(ResistanceType.Energy,   80, 90);

            // Skills
            SetSkill(SkillName.MagicResist, 95.0, 100.0);
            SetSkill(SkillName.Magery,      100.0, 110.0);
            SetSkill(SkillName.EvalInt,     100.0, 110.0);
            SetSkill(SkillName.Meditation,  90.0,  100.0);
            SetSkill(SkillName.Tactics,     90.0,  100.0);
            SetSkill(SkillName.Wrestling,   90.0,  100.0);
            SetSkill(SkillName.SpiritSpeak, 90.0,  100.0);

            Fame           = 15000;
            Karma         = -15000;
            VirtualArmor   = 65;

            SetWeaponAbility(WeaponAbility.ConcussionBlow);
        }

        public Velmara(Serial serial) : base(serial) { }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            // Always check Combatant is Mobile before using Mobile members
            if (Combatant is Mobile target && !target.Deleted && target.Map == Map && InLOS(target))
            {
                // Arcane Tempest: big AoE wind‑up
                if (DateTime.UtcNow >= _nextTempest && InRange(target, 10))
                {
                    BeginArcaneTempest();
                    _nextTempest = DateTime.UtcNow + TimeSpan.FromSeconds(30.0);
                }

                // Mana Drain: steal a bit of the target’s mana
                if (DateTime.UtcNow >= _nextManaDrain && InRange(target, 8))
                {
                    DrainManaFrom(target);
                    _nextManaDrain = DateTime.UtcNow + TimeSpan.FromSeconds(20.0);
                }

                // Tactical teleport to keep pressure on the player
                if (DateTime.UtcNow >= _nextTeleport && Utility.RandomDouble() < 0.3)
                {
                    TeleportTo(target);
                    _nextTeleport = DateTime.UtcNow + TimeSpan.FromSeconds(40.0);
                }

                // Summon minor arcane spawns
                if (DateTime.UtcNow >= _nextSummon && Utility.RandomDouble() < 0.2)
                {
                    SummonArcaneMinions();
                    _nextSummon = DateTime.UtcNow + TimeSpan.FromSeconds(60.0);
                }
            }
        }

        private void BeginArcaneTempest()
        {
            // Wind‑up emote + animation
            PublicOverheadMessage(MessageType.Emote, 0x489, false, "*Velmara begins channeling an arcane tempest!*");
            Animate(12, 5, 1, true, false, 0); // Mage cast stance
            PlaySound(0x1F3);

            // After 3 seconds, unleash the AoE
            Timer.DelayCall(TimeSpan.FromSeconds(3.0), () =>
            {
                if (Deleted) return;

                var effectLoc = EffectItem.Create(Location, Map, TimeSpan.FromSeconds(2.0));
                Effects.SendLocationParticles(effectLoc, 0x376A, 9, 32, 0x481, 2, 9962, 0);

                foreach (Mobile m in GetMobilesInRange(10))
                {
                    if (m == this || !CanBeHarmful(m)) continue;

                    DoHarmful(m);
                    int dmg = Utility.RandomMinMax(40, 60);
                    AOS.Damage(m, this, dmg, 0, 0, 0, 0, 100);
                }
                PlaySound(0x208);
            });
        }

        private void DrainManaFrom(Mobile target)
        {
            if (target.Deleted || target.Map != Map) return;

            int steal = Utility.RandomMinMax(30, 50);
            if (target.Mana < steal) steal = target.Mana;

            if (steal > 0)
            {
                target.Mana -= steal;
                this.Mana   = Math.Min(this.Mana + steal, this.ManaMax);

                target.SendAsciiMessage("You feel your life force fade!");
                this.PublicOverheadMessage(MessageType.Emote, 0x489, false, "*Velmara siphons your mana!*");
                PlaySound(0x1FC);
                Effects.SendTargetParticles(target, 0x3728, 8, 20, 5047, EffectLayer.Head);
            }
        }

        private void TeleportTo(Mobile target)
        {
            if (target.Deleted || target.Map != Map) return;

            Point3D oldLoc = Location;
            Map oldMap    = Map;

            PlaySound(0x1FE);
            Effects.SendLocationParticles(EffectItem.Create(oldLoc, oldMap, TimeSpan.FromSeconds(1.0)),
                                          0x3728, 10, 10, 5023, 0, 9962, 0);

            MoveToWorld(target.Location, target.Map);

            PlaySound(0x1FE);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, TimeSpan.FromSeconds(1.0)),
                                          0x3728, 10, 10, 5023, 0, 9962, 0);
        }

        private void SummonArcaneMinions()
        {
            PublicOverheadMessage(MessageType.Emote, 0x489, false, "*Velmara rends space and summons arcane minions!*");
            PlaySound(0x22F);

            for (int i = 0; i < 2; i++)
            {
                Point3D loc = new Point3D(
                    X + Utility.RandomList(-2, -1, 1, 2),
                    Y + Utility.RandomList(-2, -1, 1, 2),
                    Z);

                var minion = new ArcaneDaemon();
                minion.Team = this.Team;
                minion.MoveToWorld(loc, Map);
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            // 30% chance to apply arcane burn on melee hit
            if (0.3 > Utility.RandomDouble())
            {
                if (defender != null && !defender.Deleted && defender is Mobile m)
                {
                    m.ApplyPoison(this, Poison.Lesser);
                    m.SendAsciiMessage("Velmara's touch burns you with arcane energy!");
                    PlaySound(0x1FA);
                }
            }
        }

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel    => 5;
        public override int Meat                => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, 2);

            // 10% chance for Velmara's Heart (new unique drop)
            if (Utility.RandomDouble() < 0.1)
                PackItem(new SpinebraidBodice());
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
        }
    }
}
